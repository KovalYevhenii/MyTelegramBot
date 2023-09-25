using MyTelegramBot.Controller.BotLogic;
using MyTelegramBot.Controller.DBase;
using MyTelegramBot.Controller.Handler;
using MyTelegramBot.Models.Handler;
using MyTelegramBot.Models.MessageHandler;
using MyTelegramBot.Models.PathProvider;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace MyTelegramBot.View
{
    internal class TelegramBotHandler : IHandlePollingErrorAsync, IHandleUpdateAsync
    {

        private static ChooseMenu? _chooseMenu;
        private static IUserRepository? _userRepository;
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                _chooseMenu ??= new ChooseMenu(botClient, update.Message.Chat);
            }
            _userRepository ??= new UserRepository(Constants.ConnectionString, botClient, update);

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            if (update.Message != null && _chooseMenu != null)
                            {
                                if (_chooseMenu.GetMenuState() == ChooseMenu.MenuState.StateE || _chooseMenu.GetMenuState() == ChooseMenu.MenuState.StateG)
                                {
                                    if (update.Message?.Text != null)
                                    {
                                        var userInput = update.Message.Text.Trim();
                                  
                                        UserRepository repository = new(Constants.ConnectionString, botClient, update);
                                        Validator validator = new(botClient, update.Message.Chat);

                                        await repository.AddResource(userInput,validator);
                                    }
                                }

                                else
                                {
                                    await BotOnMessageRecieving(botClient, update.Message);
                                }

                                if (update.Message?.Document != null && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.DownloadE//can be handlled in method
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
                                    await file.DownloadDocument();
                                }
                            }
                            break;
                        }

                    case UpdateType.CallbackQuery:
                        {
                            if (update.CallbackQuery != null && _chooseMenu != null)
                            {
                                ChooseMenuController controller = new(_chooseMenu);

                                await controller.HandleCallbackQueryAsync(update, update.CallbackQuery);

                                if (update.CallbackQuery.Data == "balanceE" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.BalanceE)
                                {
                                    await _userRepository.UpdateBalanceElec();
                                }
                                if (update.CallbackQuery.Data == "balanceG" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.BalanceG)
                                {
                                    await _userRepository.UpdateBalanceGas();
                                }

                            }
                            else
                            {
                                throw new NullReferenceException();
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

        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception.ToString();
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}

