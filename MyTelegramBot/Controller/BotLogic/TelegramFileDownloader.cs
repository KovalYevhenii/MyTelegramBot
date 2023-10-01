using MyTelegramBot.Models.MessageHandler;
using MyTelegramBot.Models.PathProvider;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot.Controller.BotLogic
{
    public class TelegramFileDownloader
    {
        private readonly IMessageSender _messageSender;
        private readonly IFilePathProvider _filePathProvider;
        private readonly ITelegramBotClient _botClient;
        private readonly Update _update;

        public event Action? DownloadStarted;
        public event Action? DownloadCompleted;

        public TelegramFileDownloader(ITelegramBotClient botClient, Update update, IMessageSender messageSender, IFilePathProvider filePathProvider)
        {
            _botClient = botClient;
            _update = update;
            _filePathProvider = filePathProvider;
            _messageSender = messageSender;
        }

        public async Task DownloadDocument()
        {
            var document = _update?.Message?.Document;
            try
            {
                if (document != null)
                {
                      OnDownloadStarted();

                    var fileId = document.FileId;
                    var fileInfo = await _botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    string fileName = $"{DateTime.Now:dd.MM.yy}_{document.FileName}";

                    string destinationFilePath = _filePathProvider.GetDestinationFilePath(fileName);

                    if (System.IO.File.Exists(destinationFilePath))
                    {
                        await SendInfoMessage("File already exists. Skipping download.");
                    }
                    else
                    {
                        await using Stream fileStream = System.IO.File.Create(destinationFilePath);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            await _botClient.DownloadFileAsync(filePath, fileStream);
                            OnDownloadCompleted();
                        }
                        else
                        {
                            await SendErrorMessage("Invalid File path");
                        }
                    } 
                }
                else
                {
                    await SendErrorMessage("Document was null or not provided.");
                }
            }
            catch (Exception ex)
            {
                await SendErrorMessage($"An error occurred: {ex.Message}");
            }
        }

        private async Task SendErrorMessage(string errorMessage)
        {
            await _messageSender.SendTextMessageAsync(_botClient, _update.Message!.Chat.Id, errorMessage);
        }

        private async Task SendInfoMessage(string infoMessage)
        {
            await _messageSender.SendTextMessageAsync(_botClient, _update.Message!.Chat.Id, infoMessage);
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
