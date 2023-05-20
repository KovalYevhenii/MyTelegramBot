using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.BotLogic
{
    public class KeyBordBuilder
    {
       public static InlineKeyboardMarkup CreateKeyboard()
        {
            var KeyboardButtons = new[]
            {
                new []
                {
                   InlineKeyboardButton.WithCallbackData("Handle Resources")
                }
            };
            return new InlineKeyboardMarkup(KeyboardButtons);
            
        }
    }
}
