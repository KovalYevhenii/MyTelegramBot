using Telegram.Bot;
using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using System.Security.Cryptography.X509Certificates;
using MyTelegramBot.BotLogic;

namespace MyTelegramBot
{
    class Programm
    {
        static ITelegramBotClient botClient = new TelegramBotClient("5617391285:AAH28H0P9UYLh6LgKtz12lz4IxIrn3hDmjk");
        static TelegramBot bot = new();
        static CancellationToken cancellationToken = default;

        static void Main(string[] args)
        {
            Console.WriteLine($"====Bot {botClient.GetMeAsync().Result.FirstName} started====\n");
            
            var recieverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            CancellationTokenSource cts = new();
            botClient.StartReceiving(
                bot.HandleUpdateAsync,
               bot.HandleErrorAsync,
                recieverOptions,
                cancellationToken
                );

            Console.WriteLine("Press enter to stop My Bot");
            Console.ReadLine();
            cts.Cancel();

        }
    }
}
