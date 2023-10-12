using Telegram.Bot;
using Telegram.Bot.Types;
namespace MyTelegramBot.Controller.BotLogic;

public class Validator
{
    private ITelegramBotClient _botClient;
    private Chat _chat;
    public Validator(ITelegramBotClient botClient, Chat chat)
    {
        _botClient = botClient;
        _chat = chat;
    }
    public static int? InputValidator(string input)
    {
        if (input.StartsWith("SE") || input.StartsWith("SG"))
        {
            string numericPart = input.Substring(2);

            if (int.TryParse(numericPart, out int amount) && amount > 0)
            {
                return amount;
            }
        }
        return null;
    }
    public async Task HandleValidationFailureAsync()
    {
        await _botClient.SendTextMessageAsync(_chat.Id, "Sorry, your input was incorrect");
    }
    public async Task HandleValidationSuccessAsync()
    {
        await _botClient.SendTextMessageAsync(_chat.Id, "Stored! Have nice day:)");
    }
}