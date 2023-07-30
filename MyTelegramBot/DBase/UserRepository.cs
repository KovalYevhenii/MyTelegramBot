

using Dapper;
using Npgsql;
using System.ComponentModel.Design;
using Telegram.Bot.Types;

namespace MyTelegramBot.DBase
{
    internal class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddResource(string input, Update update)
        {
          
            var userId = update.Message.From.Id;//1053778618
            string resourceType;

            if (input.StartsWith("SE"))
            {
              string numInput = new(input.Where(char.IsDigit).ToArray());
                if(int.TryParse(numInput, out int amount))
                {

                using var con = new NpgsqlConnection(_connectionString);
                    con.Open();
                    resourceType = "Electricity";

                    string insertQuery = $"INSERT INTO resources(electricity) values({amount})";
                    con.Execute(insertQuery);
                    Console.WriteLine("Value added");
                }
            }


        }


        public void GetInfo(int id)
        {
            throw new NotImplementedException();
        }

        public void RemovePreviousState(int id)
        {
            throw new NotImplementedException();
        }
        public void UpdateState(int id, int newValue)
        {
            throw new NotImplementedException();
        }
        public void BalanceCounter()
        {
            throw new NotImplementedException();
        }
        public void InputValidator(string input)
        {

        }

    }


}

