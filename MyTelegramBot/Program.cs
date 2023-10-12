using MyTelegramBot.View;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace MyTelegramBot;
class Programm
{
    static readonly string _token = File.ReadAllText("C:\\Users\\koval\\Desktop\\Projekts\\MyTelegramBotApp\\MyTelegramBot\\token.txt");
    private static async Task Main(string[] args)
    {
        CancellationTokenSource cts = new();
        var bot = new TelegramBotClient(_token);
        var me = await bot.GetMeAsync();
        
        Console.WriteLine($"==== {me.FirstName} started====\n");

        bot.StartReceiving(
               updateHandler: TelegramBotHandler.HandleUpdateAsync,
               pollingErrorHandler: TelegramBotHandler.HandlePollingErrorAsync,
               receiverOptions: new ReceiverOptions()
               {
                   AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
               },
               cancellationToken: cts.Token
           );
        Console.WriteLine("Press enter to stop My Bot");
        Console.ReadLine();
        cts.Cancel();
    }
}

