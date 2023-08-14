
using MyTelegramBot.DBase;
using System.Text.RegularExpressions;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.BotLogic
{
    public class ChooseMenu
    {
        public enum MenuState
        {
            Main,
            Settings,
            DownloadE,
            DownloadG,
            StateE,
            StateG,
            BalanseE
        }
        private MenuState _menuState;
        public MenuState GetMenuState()
        {
            return _menuState;
        }
        private ITelegramBotClient BotClient { get; set; }
        private Chat Chat { get; set; }
        public ChooseMenu(ITelegramBotClient botClient, Chat chat)
        {
            this.BotClient = botClient;
            this.Chat = chat;
        }
        public async Task StartMenu()
        {
            _menuState = MenuState.Main;
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

            await BotClient.SendTextMessageAsync(Chat.Id,
                    $"Choose what you want to do",
                    replyMarkup: inlineKeyboard);
        }
        public async Task OnAnswer(Update update, CallbackQuery callbackQuery)
        {
            _menuState = MenuState.Settings;

            switch (callbackQuery.Data)
            {
                case "elec":
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
                        await BotClient.SendTextMessageAsync(Chat, "Choose option", replyMarkup: inlineKeyboard);

                        break;
                    }
                case "gas":
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

                        await BotClient.SendTextMessageAsync(Chat, "Choose option", replyMarkup: inlineKeyboard);

                        break;
                    }
                case "downloadElec":
                    {
                        _menuState = MenuState.DownloadE;
                        await BotClient.SendTextMessageAsync(Chat, "Please send me your screenshot as Dokument for downloading.");
                        break;
                    }
                case "downloadGas":
                    {
                        _menuState = MenuState.DownloadG;
                        await BotClient.SendTextMessageAsync(Chat, "Please send me your screenshot as Dokument for downloading.");
                        break;
                    }
                case "stateElec":
                    {
                        _menuState = MenuState.StateE;
                        if (update.CallbackQuery != null)
                        {
                            await BotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, " Instruction:\n Enter keyword- SE then provide a state", showAlert: true);
                        }

                        break;
                    }
                case "stateGas":
                    {
                        _menuState = MenuState.StateG;
                        if (update.CallbackQuery != null)
                        {
                            await BotClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id, " Instruction:\n Enter keyword- SG then provide a state", showAlert: true);
                        }

                        break;
                    }
                case "balanceE":
                    {
                        await BotClient.SendTextMessageAsync(Chat, "Here are your balance");
                        break;
                    }
                case "balanceG":
                    {
                        await BotClient.SendTextMessageAsync(Chat, "Here are your balance");
                        break;
                    }

                case "return":
                    {
                        _menuState = MenuState.Main;
                        await StartMenu();
                        break;
                    }
            }

        }
    }
}
