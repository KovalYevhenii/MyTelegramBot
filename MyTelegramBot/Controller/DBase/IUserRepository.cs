using MyTelegramBot.Controller.BotLogic;
namespace MyTelegramBot.Controller.DBase;
interface IUserRepository
{
    public Task AddUser();
    public Task AddResource(string input, Validator validator);
    public Task UpdateBalanceElec();
    public Task UpdateBalanceGas();
    public Task<DateTime?> RemoveLastAddedValue();
    public Task<int> GetPreviousBalanceElec();
    public Task<int> GetPreviousBalanceGas();
    public Task<int> MonthlyBalanceOutput(string resuorceType);
    public Task<int> TotalBalanceOutput(string resourceType);
    public Task<List<int>> YearValuesElectricity(string resourceType);
}
