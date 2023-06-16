using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot
{
    public class TelegramFileDownloader
    {
        private readonly IMessageSender _messageSender;
        public string? baseFolderPath = @"C:\Users\koval\Desktop\resources\";
        public TelegramFileDownloader(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }
        //
        public async Task Download(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            if (update.Message?.Document != null)
            {
                try
                {
                    var fileId = update.Message.Document.FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId, cancellationToken: cancellationToken);
                    var filePath = fileInfo.FilePath;
                    string fileName = $"{DateTime.Now:dd.MM.yy}_{update.Message.Document.FileName}";

                    string destinationFilePath = Path.Combine(baseFolderPath, fileName);

                    await using Stream fileStream = System.IO.File.Create(destinationFilePath);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        await botClient.DownloadFileAsync(filePath, fileStream, cancellationToken);
                        await _messageSender.SendTextMessageAsync(botClient, update.Message.Chat.Id, "Document downloaded!");
                    }
                    else
                    {
                        await _messageSender.SendTextMessageAsync(botClient, update.Message.Chat.Id, "Ivalid File path");
                    }
                }
                catch (Exception ex)
                {
                    await _messageSender.SendTextMessageAsync(botClient, update.Message.Chat.Id, $"An error occurred: {ex.Message}");
                }
            }
            else
            {
                await _messageSender.SendTextMessageAsync(botClient, update.Message.Chat.Id, "Document was null or not provided.");
            }
        }
    }
}
