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
        public Task AddResource(string input, IMessageSender message);
        public Task GetInfo(int id);
        public Task UpdateBalanceElec();
        public Task UpdateBalanceGas();
        public Task<int> GetPreviousBalanceElec();
        public Task<int> GetPreviousBalanceGas();

        public Task<bool> Deleteresoure(string resourceName);
    }
}
