namespace MyTelegramBot.Models.PathProvider;
public interface IFilePathProvider
{
    string GetDestinationFilePath(string fileName);
}
