

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
            _botClient = botClient;
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task AddResource(string input,  Update update, IMessageSender message)
        {
            var userId = update.Message?.From?.Id;
            var amount = InputValidator(input);
          
            try
            { 
                if (amount.HasValue && amount.Value > 0 && userId.HasValue)
                {
                    using (var con = new NpgsqlConnection(_connectionString))
                    {
                        string insertQuery = "INSERT INTO resources(electricity,user_id) values(@amount,@userId)";
                        await con.OpenAsync();
                        using (var cmd = new NpgsqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@amount", amount.Value);
                            cmd.Parameters.AddWithValue("@userID", userId);

                            await cmd.ExecuteNonQueryAsync();
                        }
                       con.Close();

                        await message.SendTextMessageAsync(_botClient,update.Message.Chat.Id, "Stored! Have nice day:)");
                    }
                }
                else
                {
                    await message.SendTextMessageAsync(_botClient, update.Message.Chat.Id, "Sorry, your input was incorrect");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
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

        private int? InputValidator(string input)
        {
            if (input.StartsWith("SE") || input.StartsWith("SG"))
            {
                input = new(input.Where(char.IsDigit).ToArray());

                if (int.TryParse(input, out int amount))
                {
                    return amount;
                }
            }
            return null;
        }
    }
}

