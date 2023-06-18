using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MyTelegramBot
{
    
    public interface IMessageSender
    { 
        Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string message);
    }
}
