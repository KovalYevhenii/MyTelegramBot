using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot.DBase
{
    internal class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public List<Resources>? Resources { get; set; }
    }
    internal class Resources
    {
        public int Id { get; set; }
        public string? ResourceType { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
    }
}
