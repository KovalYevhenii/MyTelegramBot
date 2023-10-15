using Telegram.Bot;
namespace MyTelegramBot.Models.MessageHandler;
public interface IMessageSender
{
    Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string message);
}
