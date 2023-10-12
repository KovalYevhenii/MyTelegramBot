using Dapper;
using MyTelegramBot.Controller.BotLogic;
using Npgsql;
using Telegram.Bot.Types;
namespace MyTelegramBot.Controller.DBase;
internal class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly long _userId;
    public UserRepository(string connectionString, Update update)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _userId = update?.Message?.From?.Id ?? 0;
    }
    public async Task AddResource(string input, Validator validator)
    {
        var amount = Validator.InputValidator(input);
        try
        {
            if (amount != null && amount.Value > 0)
            {
                var tableName = input.StartsWith("SE") ? "electricity" : input.StartsWith("SG") ? "gas" : null;
  
                if (tableName != null)
                {
                    using (var con = new NpgsqlConnection(_connectionString))
                    {
                        var query = $"INSERT INTO resources({tableName},user_id) VALUES(@amount,@userId)";
                        await con.OpenAsync();

                        using (var cmd = new NpgsqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@amount", amount.Value);
                            cmd.Parameters.AddWithValue("@userID", _userId);

                            await cmd.ExecuteNonQueryAsync();
                            await validator.HandleValidationSuccessAsync();
                        }
                    }
                }
            }
          
        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public async Task UpdateBalanceElec()
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                string selectQuery = $"SELECT electricity FROM resources where electricity is not NULL Order By electricity desc limit 1";
                await con.OpenAsync();

                var currentResourceValue = await con.QueryFirstOrDefaultAsync<int>(selectQuery);
                var previousBalanse = await GetPreviousBalanceElec();
                if (currentResourceValue == 0)
                    currentResourceValue = previousBalanse;
                var difference = currentResourceValue - previousBalanse;

                string updateMontlyBalanceQuery = $"UPDATE users SET balance_electricity = {difference} WHERE user_id = {_userId}";
                string yearBalanceInsertQuery = $"INSERT INTO  resources(year_balance_electricity, user_id) VALUES({difference},{_userId})";
                await con.ExecuteAsync(updateMontlyBalanceQuery);
                await con.ExecuteAsync(yearBalanceInsertQuery);
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
                string selectQuery = $"SELECT gas FROM resources where gas is not NULL Order By gas desc limit 1";
                await con.OpenAsync();
                var currentResourceValue = await con.QueryFirstOrDefaultAsync<int?>(selectQuery);

                var previousBalanse = await GetPreviousBalanceGas();
                if (currentResourceValue == 0)
                    currentResourceValue = previousBalanse;

                var difference = currentResourceValue - previousBalanse;

                string updateMontlyBalanceQuery = $"UPDATE users SET balance_gas = {difference} WHERE user_id = {_userId}";
                string yearBalanceInsertQuery = $"INSERT INTO resources(year_balance_gas, user_id) VALUES({difference},{_userId})";
                await con.ExecuteAsync(updateMontlyBalanceQuery);
                await con.ExecuteAsync(yearBalanceInsertQuery);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public async Task<int> MonthlyBalanceOutput(string resuorceType)
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                await con.OpenAsync();
                string selectQuery = $"SELECT {resuorceType} FROM users WHERE user_id = {_userId}";

                var balance = await con.QueryFirstOrDefaultAsync<int>(selectQuery);
                return balance;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            return 0;
        }
    }
    public async Task<int> GetPreviousBalanceGas()
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                await con.OpenAsync();

                var selectQuery = $"SELECT gas FROM resources WHERE gas is not null ORDER BY gas desc LIMIT 1 offset 1";

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
                var selectQuery = $"SELECT electricity FROM resources WHERE electricity is not null ORDER BY electricity desc LIMIT 1 offset 1";

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

    public async Task<bool> DeleteResoure(string resourceName)
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                var selectQuery = $"SELECT{resourceName},MAX(timestamp)from resources GROUP BY{resourceName}";
                DateTime? lastTimeStamp = await con.ExecuteScalarAsync<DateTime>(selectQuery);

                if (lastTimeStamp.HasValue)
                {
                    string deleteQuery = "DELETE from resources where timestamp = @lastTimeStamp";
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

    public async Task AddUser()
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                var selectQuery = $"select add_user({_userId})";
                await con.OpenAsync();
                await con.ExecuteAsync(selectQuery);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("User validation procedure" + ex.Message);
        }
    }

    public async Task<int> TotalBalanceOutput(string resourceType)
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                var selectCurrentQuery = $"SELECT {resourceType} FROM resources where {resourceType} is not NULL Order By {resourceType} desc limit 1";
                var selectFirstAddedQuery = $"SELECT {resourceType} FROM resources where {resourceType} is not NULL Order By {resourceType} asc limit 1";
                await con.OpenAsync();
                var currentValue = await con.ExecuteScalarAsync<int>(selectCurrentQuery);
                var firstAddedValue = await con.ExecuteScalarAsync<int>(selectFirstAddedQuery);
                var result = currentValue - firstAddedValue;
                return result;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<int>> YearValuesElectricity(string resourceType)
    {
        try
        {
            using (var con = new NpgsqlConnection(_connectionString))
            {
                var selectYearBalanceQuery = $"select {resourceType} from resources where {resourceType} is not null";
                await con.OpenAsync();
                var result = await con.QueryAsync<int>(selectYearBalanceQuery);
                var resultList = result.ToList();
                return resultList;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}