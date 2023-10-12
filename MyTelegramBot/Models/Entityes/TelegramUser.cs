namespace MyTelegramBot.Models.Entityes;
internal class TelegramUser
{
    public int user_id { get; set; }
    public string? user_name { get; set; }
    public int balance_electricity { get; set; }
    public int balance_gas { get; set; }
    public int year_balance_electricity { get;set; }
    public int year_balance_gas { get;set; }
}
