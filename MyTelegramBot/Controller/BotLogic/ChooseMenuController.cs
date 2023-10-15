using MyTelegramBot.View;
using Telegram.Bot.Types;
namespace MyTelegramBot.Controller.BotLogic;
internal class ChooseMenuController
{
    private readonly ChooseMenu _chooseMenu;
    public ChooseMenuController(ChooseMenu chooseMenu)
    {
        _chooseMenu = chooseMenu;
    }
    public async Task HandleCallbackQueryAsync(Update update, CallbackQuery callbackQuery)
    {
        switch(callbackQuery.Data)
        {
            case "statistic":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.Main);
                    await _chooseMenu.ChartMessage();
                    break;
                }
            case "elec":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.Main);
                    await _chooseMenu.HandleElecOptionAsync();
                    break;
                }
            case "gas":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.Main);
                    await _chooseMenu.HandleGasOptionAsync();
                    break;
                }
            case "remove":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.Main);
                    break;
                }
            case "downloadElec":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.DownloadE);
                    await _chooseMenu.HandleDownloadElecAsync();
                    break;
                }
            case "downloadGas":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.DownloadG);
                    await _chooseMenu.HandleDownloadGasAsync();
                    break;
                }
            case "stateElec":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.StateE);
                    await _chooseMenu.HandleStateElecAsync(update);
                    break;
                }
            case "stateGas":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.StateG);
                    await _chooseMenu.HandleStateGasAsync(update);
                    break;
                }
            case "balanceE":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.BalanceE);
                    await _chooseMenu.HandleBalanceAsync();
                    break;
                }
            case "balanceG":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.BalanceG);
                    await _chooseMenu.HandleBalanceAsync();
                    break;
                } 
            case "YearBalanceE":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.YearBalanceE);
                    await _chooseMenu.HandleBalanceAsync();
                    break;
                } 
            case "YearBalanceG":
                {
                    _chooseMenu.SetMenuState(ChooseMenu.MenuState.YearBalanceG);
                    await _chooseMenu.HandleBalanceAsync();
                    break;
                }
            case "return":
                {
                    await _chooseMenu.HandleReturnAsync();
                    break;
                }
        }
    }

}
