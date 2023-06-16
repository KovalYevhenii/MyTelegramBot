using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.PathProvider
{
    public class FilePathProvider : IFilePathProvider
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
}
