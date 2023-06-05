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
        private ITelegramBotClient _botClient { get; set; }
        private Chat _chat { get; set; }
        public ChooseMenu(ITelegramBotClient botClient, Chat chat)
        {
            _botClient = botClient;
            _chat = chat;
        }

        public  async Task StartMenu()
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
                        await _botClient.SendTextMessageAsync(_chat, "Choose option",replyMarkup: inlineKeyboard);

                        break;
                    }


            }
        }
    }
}
