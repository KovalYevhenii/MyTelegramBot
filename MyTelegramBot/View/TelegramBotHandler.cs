using MyTelegramBot.Controller.BotLogic;
using MyTelegramBot.Controller.DBase;
using MyTelegramBot.Controller.Handler;
using MyTelegramBot.Models.Handler;
using MyTelegramBot.Models.MessageHandler;
using MyTelegramBot.Models.PathProvider;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace MyTelegramBot.View;
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
        _userRepository ??= new UserRepository(Constants.ConnectionString, update);

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
                                    Validator validator = new(botClient, update.Message.Chat);
                                    await _userRepository.AddResource(userInput, validator);

                                    if (userInput.StartsWith("SE"))
                                    {
                                        await _userRepository.UpdateBalanceElec();
                                    }
                                    else if (userInput.StartsWith("SG"))
                                    {
                                        await _userRepository.UpdateBalanceGas();
                                    }
                                    else
                                    {
                                        await validator.HandleValidationFailureAsync();
                                    }
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
                                    await botClient.SendTextMessageAsync(update.Message!.Chat.Id, "Download Completed!");
                                };
                                await file.DownloadDocument();
                            }
                        }
                        break;
                    }

                case UpdateType.CallbackQuery:
                    {
                        var chatId = update.CallbackQuery!.Message!.Chat.Id;

                        if (update.CallbackQuery != null && _chooseMenu != null)
                        {
                            ChooseMenuController controller = new(_chooseMenu);

                            await controller.HandleCallbackQueryAsync(update, update.CallbackQuery);

                            if (update.CallbackQuery.Data == "balanceE" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.BalanceE)
                            {
                                var res = await _userRepository.MonthlyBalanceOutput("balance_electricity");
                                await botClient.SendTextMessageAsync(chatId, $"{string.Join(' ', res)}Kw", cancellationToken: cancellationToken);
                            }
                            if (update.CallbackQuery.Data == "balanceG" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.BalanceG)
                            {
                                var res = await _userRepository.MonthlyBalanceOutput("balance_gas");
                                await botClient.SendTextMessageAsync(chatId, $"{string.Join(' ', res)}Kw");
                            }
                            if (update.CallbackQuery.Data == "YearBalanceE" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.YearBalanceE)
                            {
                                var res = await _userRepository.TotalBalanceOutput("electricity");
                                await botClient.SendTextMessageAsync(chatId, $"{string.Join(' ', res)}Kw");
                            }
                            if (update.CallbackQuery.Data == "YearBalanceG" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.YearBalanceG)
                            {
                                var res = await _userRepository.TotalBalanceOutput("gas");
                                await botClient.SendTextMessageAsync(chatId, $"{string.Join(' ', res)}Kw");
                            }
                            if (update.CallbackQuery.Data == "statistic" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.Main)
                            {
                                var resElectricity = await _userRepository.YearValuesElectricity("year_balance_electricity");
                                var resGas = await _userRepository.YearValuesElectricity("year_balance_gas");
                                await botClient.SendTextMessageAsync(chatId, "Electricity consumption in Kw\t" + Chart.DisplayBarVerticalChart(resElectricity));
                                await botClient.SendTextMessageAsync(chatId, "Gas consumption in Kw\t" + Chart.DisplayBarVerticalChart(resGas));
                            }
                            if (update.CallbackQuery.Data == "remove" && _chooseMenu.GetMenuState() == ChooseMenu.MenuState.Main)
                            {
                                var deletedTimestamp = await _userRepository.RemoveLastAddedValue();
                                var deletedTimestamp2 = await _userRepository.RemoveLastAddedValue();
                                if (deletedTimestamp != DateTime.MinValue && deletedTimestamp2.Value != DateTime.MinValue)
                                {
                                    await _chooseMenu.ShowDeletedTimestamp(deletedTimestamp);
                                    await _chooseMenu.ShowDeletedTimestamp(deletedTimestamp2);
                                } 
                                else
                                    await botClient.SendTextMessageAsync(chatId,"There is no values to Delete");
                            }
                        }
                        else
                        {
                            throw new NullReferenceException("Update callbackquery was null");
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
                    await _userRepository.AddUser();
                    await menu.StartMenu();
                    break;
                }
            default:
                {
                    await menu.StartMessage(botClient, chatId);
                    break;
                }
        }
    }
    public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception.ToString();
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}

