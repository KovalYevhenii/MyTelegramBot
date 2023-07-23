
namespace MyTelegramBot.PathProvider
{
    public interface IFilePathProvider
    {
        string GetDestinationFilePath(string fileName);
    }
}
