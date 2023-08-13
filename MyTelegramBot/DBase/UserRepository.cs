﻿

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

        public UserRepository(string connectionString, ITelegramBotClient botClient, Update update)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _botClient = botClient;
            _userId = update?.Message?.From?.Id ?? 0;
            _chatId = update?.Message?.Chat.Id ?? 0;
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
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    string selectQuery = $"SELECT distinct electricity FROM resources WHERE user_id = {_userId} ORDER BY electricity desc LIMIT 1 OFFSET 1";
                    var currentResourceValue = await con.QueryFirstOrDefaultAsync<int>(selectQuery);

                    var previousBalanse = await GetPreviousBalanceElec();
                    var difference = currentResourceValue - previousBalanse;

                    string updateQuery = $"UPDATE users SET balance_electricity = {difference} WHERE user_id = {_userId}";
                    await con.ExecuteAsync(updateQuery);
                }
                await BalanceOutput("balance_electricity");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        private async Task BalanceOutput(string resuorceType)
        {
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    string selectQuery = $"SELECT {resuorceType} FROM users WHERE user_id = {_userId}";
                    var balance = await con.QueryFirstOrDefaultAsync(selectQuery);

                    await _botClient.SendTextMessageAsync(_chatId, $"{balance}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        public async Task UpdateBalanceGas()
        {
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    string selectQuery = $"SELECT distinct gas FROM resources WHERE user_id = {_userId} ORDER BY gas DESC LIMIT 1 offset 1";
                    var currentResourceValue = await con.QueryFirstOrDefaultAsync<int?>(selectQuery);

                    var previousBalanse = await GetPreviousBalanceGas();
                    var difference = currentResourceValue - previousBalanse;

                    string updateQuery = $"UPDATE users SET balance_gas = {difference} WHERE user_id = {_userId}";
                    await con.ExecuteAsync(updateQuery);
                }
                await BalanceOutput("balance_gas");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        public async Task<int> GetPreviousBalanceGas()
        {
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    var selectQuery = $"SELECT distinct gas FROM resources WHERE user_id = {_userId} ORDER BY gas LIMIT 1";

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
                    var SelectQuery = $"SELECT distinct electricity FROM resources WHERE user_id = {_userId} ORDER BY electricity  LIMIT 1";

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

        public async Task<bool> Deleteresoure(string resourceName)
        {
            try
            {
                using (var con = new NpgsqlConnection(_connectionString))
                {
                    var selectQuery = $"SELECT{resourceName},MAX(timestamp)from resources GROUP BY{resourceName}";
                    DateTime? lastTimeStamp = await con.ExecuteScalarAsync<DateTime>(selectQuery);

                    if (lastTimeStamp.HasValue)
                    {
                        string deleteQuery = $"DELETE from resources where timestamp = @lastTimeStamp";
                        var rowsAffected = await con.ExecuteAsync(deleteQuery, new { lastTimeStamp });

                        return rowsAffected > 0;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }
    }
}

