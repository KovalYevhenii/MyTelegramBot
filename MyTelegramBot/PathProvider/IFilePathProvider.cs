using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.PathProvider
{
    public interface IFilePathProvider
    {
        string GetDestinationFilePath(string fileName);

    }
}
