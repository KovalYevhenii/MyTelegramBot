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
            input = new(input.Where(char.IsDigit).ToArray());

            if (int.TryParse(input, out int amount))
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
