using MyTelegramBot.BotLogic;
using MyTelegramBot.DBase;
using MyTelegramBot.MessageHandler;
using MyTelegramBot.PathProvider;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot
{
    class Programm
    {
        static readonly string _token = System.IO.File.ReadAllText("C:\\Users\\koval\\Desktop\\Projekts\\MyTelegramBotApp\\MyTelegramBot\\token.txt");

        private static ChooseMenu? _chooseMenu;
        private static IUserRepository? _userRepository;

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception.ToString();
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
        private static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new();
            var bot = new TelegramBotClient(_token);
            var me = await bot.GetMeAsync();
            Console.WriteLine($"====Bot {me.FirstName} started====\n");

            bot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
                },
                cancellationToken: cts.Token
            );

            Console.WriteLine("Press enter to stop My Bot");

            Console.ReadLine();

            cts.Cancel();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _chooseMenu ??= new ChooseMenu(botClient, update.Message.Chat);
            _userRepository ??= new UserRepository(Constants.ConnectionString, botClient, update);

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            if (update.Message != null)
                            {
                                if (_chooseMenu.GetMenuState() == ChooseMenu.MenuState.StateE)
                                {
                                    if (update.Message?.Text != null)
                                    {
                                        var userInput = update.Message.Text.Trim();

                                        UserRepository repository = new(Constants.ConnectionString, botClient, update);

                                        IMessageSender message = new TelegramMessageSender();

                                        await repository.AddResource(userInput, message);
                                    }
                                }

                                else
                                {
                                    await BotOnMessageRecieving(botClient, update.Message);
                                }

                                if (update.Message?.Document != null && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.DownloadE//МОЖНО ЗАКИНУТЬ В ОТДЕЛЬНЫЙ МЕТОД
                                    || _chooseMenu.GetMenuState() == ChooseMenu.MenuState.DownloadG)
                                {
                                    IMessageSender messageSender = new TelegramMessageSender();

                                    IFilePathProvider filePathProvider = new DefaultFilePathProvider();

                                    if (_chooseMenu.GetMenuState() == ChooseMenu.MenuState.DownloadE)
                                    {
                                        filePathProvider = new ElecFilePathProvider();
                                    }

                                    else if (_chooseMenu.GetMenuState() == ChooseMenu.MenuState.DownloadG)
                                    {
                                        filePathProvider = new GasFilePathProvider();
                                    }

                                    bool useTelegramMessageSender = false;

                                    if (useTelegramMessageSender)
                                    {
                                        messageSender = new TelegramMessageSender();
                                    }

                                    else
                                    {
                                        messageSender = new ConsoleMessageSender();
                                    }

                                    var file = new TelegramFileDownloader(botClient, update, messageSender, filePathProvider);

                                    file.DownloadStarted += async () =>
                                    {
                                        await Console.Out.WriteLineAsync("Download Started!");
                                    };
                                    file.DownloadCompleted += async () =>
                                    {
                                        await Console.Out.WriteLineAsync("Download Completed!");
                                    };

                                    await file.Download();
                                }
                            }
                            break;
                        }

                    case UpdateType.CallbackQuery:
                        {
                            if (update.CallbackQuery != null)
                            {
                                await _chooseMenu.OnAnswer(update, update.CallbackQuery);

                                if (update.CallbackQuery.Data == "balanceE")
                                {
                                    await Console.Out.WriteLineAsync("method shold work");
                                    await _userRepository.UpdateBalanceElec();
                                }
                            }

                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                await HandlePollingErrorAsync(botClient, exception, cancellationToken);
            }
        }
        public static async Task BotOnMessageRecieving(ITelegramBotClient botClient, Message message)
        {
            ChooseMenu menu = new(botClient, message.Chat);
            var chatId = message.Chat.Id;
            Console.WriteLine($"Recieved message type{message.Type} {message.Text}");

            if (message.Type != MessageType.Text)
            {
                return;
            }

            var action = message.Text!.Split(' ')[0];

            switch (action)
            {
                case "/start":
                    {
                        await menu.StartMenu();
                        break;
                    }
                default:
                    {
                        await StartMessage(botClient, chatId);
                        break;
                    }
            }
        }
        public static async Task StartMessage(ITelegramBotClient botClient, ChatId chatId)
        {
            await botClient.SendTextMessageAsync(chatId, "Hi🥰, I was developed to make your day easier! press /start");
        }

    }

}
