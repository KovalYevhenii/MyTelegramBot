using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.View;
public class ChooseMenu
{
    private readonly ITelegramBotClient _botClient;
    private readonly Chat _chat;
    private MenuState _menuState;

    public ChooseMenu(ITelegramBotClient botClient, Chat chat)
    {
        _botClient = botClient;
        _chat = chat;
    }
    public enum MenuState
    {
        Main,
        Settings,
        DownloadE,
        DownloadG,
        StateE,
        StateG,
        BalanceE,
        BalanceG,
        YearBalanceE,
        YearBalanceG
    }
    public MenuState GetMenuState()
    {
        return _menuState;
    }
    public void SetMenuState(MenuState newState)
    {
        _menuState = newState;
    }

    public async Task StartMenu()
    {
        InlineKeyboardMarkup inlineKeyboard = new(
              new[] {
                new[]
                {
                    InlineKeyboardButton.WithUrl("check Tarif", "https://www.check24.de/strom/vergleich/check24/?totalconsumption=2000&pricecap=no&zipcode=42119&city=Wuppertal&pid=24&pricing=month&product_id=1&calculationparameter_id=772b1d39b1ebc2fdf7581760533cf2b2")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("handle electricity resources", "elec")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Handle Gas resuorces", "gas")
                },
                 new[]
                {
                    InlineKeyboardButton.WithCallbackData("Statistics", "statistic")
                }
              });

        await _botClient.SendTextMessageAsync(_chat.Id,
                $"Choose what you want to do",
                replyMarkup: inlineKeyboard);
    }
    public async Task HandleElecOptionAsync()
    {
        InlineKeyboardMarkup inlineKeyboard = new(
                   new[] {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Upload Electricity","downloadElec"),

                            InlineKeyboardButton.WithCallbackData("Enter meter state","stateElec"),

                            InlineKeyboardButton.WithCallbackData("Monthly balance", "balanceE")

                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Yearly balance", "YearBalanceE"),
                        },
                         new[]
                         {
                            InlineKeyboardButton.WithCallbackData("⏪","return")

                         }
                   });
        await _botClient.SendTextMessageAsync(_chat, "Choose option", replyMarkup: inlineKeyboard);
    }
    public async Task HandleGasOptionAsync()
    {
        InlineKeyboardMarkup inlineKeyboard = new(
                   new[] {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Upload Gas","downloadGas"),

                            InlineKeyboardButton.WithCallbackData("Enter meter state","stateGas"),

                            InlineKeyboardButton.WithCallbackData("Monthly balance", "balanceG")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Yearly balance", "YearBalanceG"),
                        },
                         new[]
                         {
                            InlineKeyboardButton.WithCallbackData("⏪","return")
                         }
                   });

        await _botClient.SendTextMessageAsync(_chat, "Choose option", replyMarkup: inlineKeyboard);
    }
    public async Task HandleDownloadElecAsync()
    {
        SetMenuState(MenuState.DownloadE);
        await _botClient.SendTextMessageAsync(_chat, "Please send me your screenshot as Dokument for downloading.");
    }
    public async Task HandleDownloadGasAsync()
    {
        SetMenuState(MenuState.DownloadG);
        await _botClient.SendTextMessageAsync(_chat, "Please send me your screenshot as Dokument for downloading.");
    }

    public async Task HandleStateElecAsync(Update update)
    {
        SetMenuState(MenuState.StateE);
        if (update.CallbackQuery != null)
        {
            await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, " Instruction:\n Enter keyword- SE then provide a state", showAlert: true);
        }
    }

    public async Task HandleStateGasAsync(Update update)
    {
        SetMenuState(MenuState.StateG);
        if (update.CallbackQuery != null)
        {
            await _botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, " Instruction:\n Enter keyword- SG then provide a state", showAlert: true);
        }
    }
    public async Task HandleBalanceAsync()
    {
        await _botClient.SendTextMessageAsync(_chat, "Here is your balance");
    }
    public async Task ChartMessage()
    {
        await _botClient.SendTextMessageAsync(_chat, "Please be patient,😅 I'm loading the graphics...");
    }

    public async Task HandleReturnAsync()
    {
        SetMenuState(MenuState.Main);
        await StartMenu();
    }
}
