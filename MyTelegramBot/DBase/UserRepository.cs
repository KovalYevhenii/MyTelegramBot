

using Dapper;
using Npgsql;
using Npgsql.Replication.PgOutput.Messages;
using System.ComponentModel.Design;
using System.Net.Sockets;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MyTelegramBot.DBase
{
    internal class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ITelegramBotClient _botClient;
        private readonly long _userId;
        private readonly long _chatId;
        private readonly string? _userName;
      
        public UserRepository(string connectionString, ITelegramBotClient botClient, Update update)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _botClient = botClient;
            _userId = update?.Message?.From?.Id ?? 0;
            _chatId = update?.Message?.Chat.Id ?? 0;
            _userName = update?.Message?.From?.Username;
        }

        public async Task AddResource(string input, IMessageSender message)
        {
            var amount = InputValidator(input);

            try
            {
                if (amount.HasValue && amount.Value > 0 && amount != null)
                {
                    var tableName = input.StartsWith("SE") ? "electricity" : input.StartsWith("SG") ? "gas" : null;
                    if (tableName != null)
                    {
                        var query = $"INSERT INTO resources({tableName},user_id) VALUES(@amount,@userId)";

                        using (var con = new NpgsqlConnection(_connectionString))
                        {

                            await con.OpenAsync();
                            using (var cmd = new NpgsqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@amount", amount.Value);
                                cmd.Parameters.AddWithValue("@userID", _userId);

                                await cmd.ExecuteNonQueryAsync();
                            }
                            con.Close();
                        }

                        await message.SendTextMessageAsync(_botClient, _chatId, "Stored! Have nice day:)");
                    }
                }
                else
                {
                    await message.SendTextMessageAsync(_botClient, _chatId, "Sorry, your input was incorrect");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public Task GetInfo(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateState(int id, int newValue)
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

        public async Task UpdateBalanceElec()
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                string selectQuery = $"SELECT electricity FROM resources WHERE user_id = {_userId} ORDER BY DESC LIMIT 1";
                var currentResourceValue = await con.QueryFirstOrDefaultAsync<int>(selectQuery);

                var previousBalanse = await GetPreviousBalanceElec();
                var difference = currentResourceValue - previousBalanse;

                string insertQuery = $"INSERT INTO users(user_id,user_name, balance_electrisity)VALUES({_userId},{_userName},{difference})";
            }

        }
        public Task UpdateBalanceGas()
        {
            throw new NotImplementedException();
        }
        public async Task<int> GetPreviousBalanceGas()
        {
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    var selectQuery = $"SELECT balance_gas FROM users WHERE user_id = {_userId} ORDER BY balance_gas DESC LIMIT 1";
                    
                    var previousBalance = await con.QueryFirstOrDefaultAsync<int>(selectQuery);

                    return previousBalance;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                return 0;
            }
        }

        public async Task<int> GetPreviousBalanceElec()
        {
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    var SelectQuery = $"SELECT balance_electricity FROM users WHERE user_id = {_userId} ORDER BY balance_electricity DESC LIMIT 1";

                    var previousBalance = await con.QueryFirstOrDefaultAsync<int>(SelectQuery);

                    return previousBalance;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

                return 0;
            }
        }

    }
}

