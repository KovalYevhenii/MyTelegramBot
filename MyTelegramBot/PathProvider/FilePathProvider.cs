
namespace MyTelegramBot.PathProvider
{
    public class DefaultFilePathProvider : IFilePathProvider
    {
        public string GetDestinationFilePath(string fileName)
        {
            string basePath = @"C:\Users\koval\Desktop\resources";
            return Path.Combine(basePath, fileName);
        }
    }
    public class ElecFilePathProvider : IFilePathProvider
    {
        public string GetDestinationFilePath(string fileName)
        {
            string basePath = @"C:\Users\koval\Desktop\resources\electricity";
            return Path.Combine(basePath, fileName);
        }
    }
    public class GasFilePathProvider : IFilePathProvider
    {
        public string GetDestinationFilePath(string fileName)
        {
            string basePath = @"C:\Users\koval\Desktop\resources\gas";
            return Path.Combine(basePath, fileName);
        }
    }
}
