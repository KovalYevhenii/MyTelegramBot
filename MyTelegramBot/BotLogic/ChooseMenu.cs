
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
            Download
        }
        private  MenuState _menuState;
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

                                InlineKeyboardButton.WithCallbackData("Enter meter state","stateElec")
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
                                InlineKeyboardButton.WithCallbackData("Download electricity meter screenshot","downloadGas"),

                                InlineKeyboardButton.WithCallbackData("Enter meter state","stateGas")
                            }
                        });

                        await BotClient.SendTextMessageAsync(Chat, "Choose option", replyMarkup: inlineKeyboard);

                        break;
                    }
                case "downloadElec":
                    {
                        _menuState = MenuState.Download;
                        await BotClient.SendTextMessageAsync(Chat, "Please send me your screenshot as Dokument for downloading.");
                      
                        break;
                    }
            }

        }

    }
}
