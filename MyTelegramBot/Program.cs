using Telegram.Bot;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Security.Cryptography.X509Certificates;

namespace MyTelegramBot
{
    class Programm
    {
        static ITelegramBotClient bot = new TelegramBotClient("5617391285:AAH28H0P9UYLh6LgKtz12lz4IxIrn3hDmjk");
        static void Main(string[] args)
        {
            Console.WriteLine($"====Bot {bot.GetMeAsync().Result.FirstName} started====\n");
            var recieverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            CancellationToken cancellationToken = default;
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                recieverOptions,
                cancellationToken
                );
            Console.ReadKey();

        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;
                if (message.Text.ToLower() == "/start")
                {
                    await HandleStartCommandAsync(botClient, chatId, cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Привет! Я создан, чтоб облегчить твой день.введи /start чтобы начать");
                }
            }
        }
        public static async Task HandleStartCommandAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                   new[]
                   {
                        InlineKeyboardButton.WithCallbackData(text: "Upload meter Data", callbackData:"11"),
                        InlineKeyboardButton.WithCallbackData(text: "Check 24", callbackData:"12"),
                   },
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "выберите действие",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
