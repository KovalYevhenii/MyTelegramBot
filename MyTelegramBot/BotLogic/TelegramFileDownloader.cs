using MyTelegramBot.PathProvider;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot
{
    public class TelegramFileDownloader
    {
        private readonly IMessageSender _messageSender;
        private readonly IFilePathProvider _filePathProvider;
        public event Action? DownloadStarted;
        public event Action? DownloadCompleted;

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
                OnDownloadStarted();

                try
                {
                    var fileId = Update.Message.Document.FileId;
                    var fileInfo = await BotClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    string fileName = $"{DateTime.Now:dd.MM.yy}_{Update.Message.Document.FileName}";

                    string destinationFilePath = _filePathProvider.GetDestinationFilePath(fileName);

                    if (System.IO.File.Exists(destinationFilePath))
                    {
                        await _messageSender.SendTextMessageAsync(BotClient, Update.Message.Chat.Id, "File already exists. Skipping download.");
                        return;
                    }

                    await using Stream fileStream = System.IO.File.Create(destinationFilePath);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        await BotClient.DownloadFileAsync(filePath, fileStream);
                        OnDownloadCompleted();
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
        protected virtual void OnDownloadStarted()
        {
            DownloadStarted?.Invoke();
        }
        protected virtual void OnDownloadCompleted()
        {
            DownloadCompleted?.Invoke();
        }
    }
}
