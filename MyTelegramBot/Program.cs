using Telegram.Bot;
using System;
using System.Threading.Tasks;
namespace MyTelegramBot
{
    class Programm
    {
        static async Task Main(string[] args)
        {
            await StartBotAsync();
        }
        static async Task StartBotAsync()
        {
            var botClient = new TelegramBotClient("5617391285:AAH28H0P9UYLh6LgKtz12lz4IxIrn3hDmjk");

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Bot is sucssesfully started user ID {me.Id}" +
                $"my name is {me.Username}");
        }
    }
}
