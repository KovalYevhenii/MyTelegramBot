

using Dapper;
using Npgsql;
using System.ComponentModel.Design;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot.DBase
{
    internal class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ITelegramBotClient _botClient;
        public UserRepository(string connectionString, ITelegramBotClient botClient)
        {
            this._botClient = botClient;
            _connectionString = connectionString;
        }

        public async void AddResource(string input, Update update, IMessageSender message)
        {
            var userId = update.Message?.From?.Id;

            try
            {
                if (input.StartsWith("SE") && update.Message != null)
                {
                    string numInput = new(input.Where(char.IsDigit).ToArray());

                    if (int.TryParse(numInput, out int amount))
                    {
                        using (var con = new NpgsqlConnection(_connectionString))
                        {
                            string insertQuery = $"INSERT INTO resources(electricity,user_id) values({amount},{userId})";

                            con.Execute(insertQuery);

                            await message.SendTextMessageAsync(_botClient, update.Message.Chat.Id, "Stored! Have nice day:)");
                        }
                    }
                    else
                    {
                        await message.SendTextMessageAsync(_botClient, update.Message.Chat.Id, "Wrong input Value:(");
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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

