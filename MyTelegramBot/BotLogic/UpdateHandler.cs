using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot.BotLogic
{
    public class UpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        public UpdateHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

       public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {

            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            if (message.Text == "/start")
            {
                var keyboard = KeyBordBuilder.CreateKeyboard();

                var replyMarkup = new ReplyKeyboardMarkup(button:"dwd");
                Message sentMessage = await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Welkome to my bot. Select a command froo the keyboard:",
                    replyMarkup: replyMarkup,
                    cancellationToken: cancellationToken);  
            }

        }
    }
}
