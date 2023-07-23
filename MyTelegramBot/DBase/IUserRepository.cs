using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.DBase
{
    interface IUserRepository
    {
        public void AddState(int id, string type, int amount);
        public void GetInfo(int id);
        public void RemovePreviousState(int id);
        public void UpdateState(int id, int newValue);
    }
}
