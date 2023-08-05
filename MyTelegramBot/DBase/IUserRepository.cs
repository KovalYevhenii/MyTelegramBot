using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MyTelegramBot.DBase
{
    interface IUserRepository
    {
        public Task AddResource(string input, Update update, IMessageSender message);
        public void GetInfo(int id);
        public void RemovePreviousState(int id);
        public void UpdateState(int id, int newValue);
    }
}
