using MyTelegramBot.PathProvider;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot
{
    public class TelegramFileDownloader
    {
        private readonly IMessageSender _messageSender;
        private readonly IFilePathProvider _filePathProvider;
        private ITelegramBotClient BotClient { get; set; }
        private Update Update { get; set; }

        public TelegramFileDownloader(ITelegramBotClient botClient, Update update, IMessageSender messageSender, IFilePathProvider filePathProvider)
        {
            this.BotClient = botClient;
            this.Update = update;
            _filePathProvider = filePathProvider;
            _messageSender = messageSender;
        }
        public async Task Download()
        {
            if (Update.Message.Document != null)
            {
                try
                {
                    var fileId = Update.Message.Document.FileId;
                    var fileInfo = await BotClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    string fileName = $"{DateTime.Now:dd.MM.yy}_{Update.Message.Document.FileName}";

                    string destinationFilePath = _filePathProvider.GetDestinationFilePath(fileName);

                    await using Stream fileStream = System.IO.File.Create(destinationFilePath);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        await BotClient.DownloadFileAsync(filePath, fileStream);
                        await _messageSender.SendTextMessageAsync(BotClient, Update.Message.Chat.Id, "Document downloaded!");
                    }
                    else
                    {
                        await _messageSender.SendTextMessageAsync(BotClient, Update.Message.Chat.Id, "Ivalid File path");
                    }
                }
                catch (Exception ex)
                {
                    await _messageSender.SendTextMessageAsync(BotClient, Update.Message.Chat.Id, $"An error occurred: {ex.Message}");
                }
            }
            else
            {
                await _messageSender.SendTextMessageAsync(BotClient, Update.Message.Chat.Id, "Document was null or not provided.");
            }
        }
    }
}
