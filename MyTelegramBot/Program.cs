
using MyTelegramBot.BotLogic;
using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


var botClient = new TelegramBotClient("5617391285:AAH28H0P9UYLh6LgKtz12lz4IxIrn3hDmjk");
using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};
var updateHandler = new UpdateHandler(botClient);

botClient.StartReceiving(
  updateHandler: update => updateHandler.HandleUpdateAsync(update, cts.Token),
   pollingErrorHandler: HandlePollingErrorAsync,
   receiverOptions: receiverOptions
 );

var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();


Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}


