using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.BotLogic
{
    public class ChooseMenu
    {
        private ITelegramBotClient _botClient { get; set; }
        private Chat _chat { get; set; }
        public ChooseMenu(ITelegramBotClient botClient, Chat chat)
        {
            this._botClient = botClient;
            this._chat = chat;
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
        public async Task OnAnswer(Update update, CallbackQuery callbackQuery)
        {
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
                        await _botClient.SendTextMessageAsync(_chat, "Choose option", replyMarkup: inlineKeyboard);

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

                        await _botClient.SendTextMessageAsync(_chat, "Choose option", replyMarkup: inlineKeyboard);

                        break;
                    }
                case "downloadElec":
                    {
                        await _botClient.SendTextMessageAsync(_chat, "Please send me your screenshot as Dokument for downloading.");

                        break;
                    }
            }

        }

    }
}
