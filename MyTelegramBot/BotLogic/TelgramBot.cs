using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.VisualBasic;

namespace MyTelegramBot.BotLogic
{
    public class TelegramBot
    {
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            try
            {

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
                        await botClient.SendTextMessageAsync(chatId, "Привет! Я создан, чтоб облегчить твой день.Нажми /start чтобы начать");
                    }
                }

                else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
                {
                    var callbackQuery = update.CallbackQuery;
                    var chatId = callbackQuery.Message?.Chat.Id;
                    var callbackData = callbackQuery.Data;
                    if (chatId != null)
                    {
                        if (callbackData == "upload")
                        {
                            await botClient.SendTextMessageAsync(chatId, "Button updload pressed");
                        } 
                        else
                        {
                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"an error occured : {ex.Message}");
            }
        }

        public static async Task HandleStartCommandAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                   new[]
                   {
                        InlineKeyboardButton.WithCallbackData(text: "Upload meter Data", callbackData:"upload"),
                        InlineKeyboardButton.WithCallbackData(text: "Check 24", callbackData:"12")
                   }
            });

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "выберите действие",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

    }
}

