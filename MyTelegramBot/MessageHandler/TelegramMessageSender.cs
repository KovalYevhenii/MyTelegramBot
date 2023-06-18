

using Telegram.Bot;

namespace MyTelegramBot
{
    internal class TelegramMessageSender:IMessageSender
    {
        public async Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
        }
    }
}
