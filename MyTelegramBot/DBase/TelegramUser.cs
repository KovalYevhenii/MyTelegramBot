using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.DBase
{
    internal class TelegramUser
    {
        public int user_id { get; set; }
        public string? user_name { get; set; }
        public int balance { get; set; }
    }
    internal class Resources
    {
        public int gas { get; set; }
        public int elctricity { get; set; }
        public int user_id { get; set; }
    }
}
