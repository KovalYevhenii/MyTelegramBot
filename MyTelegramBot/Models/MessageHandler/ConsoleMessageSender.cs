﻿using Telegram.Bot;

namespace MyTelegramBot.Models.MessageHandler;

public class ConsoleMessageSender : IMessageSender
{
    public async Task SendTextMessageAsync(ITelegramBotClient botClient, long chatId, string message)
    {
        Console.WriteLine($"Sending message to chat {chatId}:{message}");
        await botClient.SendTextMessageAsync(chatId, message);
    }
}
