using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.BotLogic
{
    public class ChooseMenu
    {
        private ITelegramBotClient _botClient;
        private Chat _chat;
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
                  }
                  );

            await _botClient.SendTextMessageAsync(_chat.Id,
                    $"Choose what you want to do",
                    replyMarkup: inlineKeyboard);
        }
        public async Task OnAnswer(CallbackQuery callbackQuery)
        {
            switch (callbackQuery.Data)
            {
                case "elec":
                    {

                        InlineKeyboardMarkup inlineKeyboard = new(
                        new[] {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Download electricity meter screenshot","download"),
                                InlineKeyboardButton.WithCallbackData("Enter meter state","state")
                            }
                        }
                       );
                        await _botClient.SendTextMessageAsync(_chat.Id, "Choose option",replyMarkup: inlineKeyboard);

                        break;
                    }


            }
        }
    }
}
