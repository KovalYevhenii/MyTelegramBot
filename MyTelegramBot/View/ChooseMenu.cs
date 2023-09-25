using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.View
{
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
            BalanceG
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
                        InlineKeyboardButton.WithUrl("check Tarif", "check.24")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("handle electricity resources", "elec")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Handle Gas resuorces", "gas")
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
                                InlineKeyboardButton.WithCallbackData("Download electricity meter screenshot","downloadElec"),

                                InlineKeyboardButton.WithCallbackData("Enter meter state","stateElec"),

                                InlineKeyboardButton.WithCallbackData("Get my State", "balanceE")

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
                                InlineKeyboardButton.WithCallbackData("Download Gas meter screenshot","downloadGas"),

                                InlineKeyboardButton.WithCallbackData("Enter meter state","stateGas"),

                                  InlineKeyboardButton.WithCallbackData("Get my State", "balanceG")
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
            await _botClient.SendTextMessageAsync(_chat, "Here are your balance");
        }

        public async Task HandleReturnAsync()
        {
            SetMenuState(MenuState.Main);
            await StartMenu();
        }
    }
}
