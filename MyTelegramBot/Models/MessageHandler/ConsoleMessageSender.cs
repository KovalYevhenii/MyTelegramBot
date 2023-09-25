using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MyTelegramBot.Models.MessageHandler
{
    public class ConsoleMessageSender : IMessageSender
    {
        public async Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string message)
        {
            Console.WriteLine($"Sending message to chat {chatId}:{message}");
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
