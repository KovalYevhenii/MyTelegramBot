﻿using MyTelegramBot.View;
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
        _chooseMenu.SetMenuState(ChooseMenu.MenuState.Settings);
        switch(callbackQuery.Data)
        {
            case "elec":
                {
                    await _chooseMenu.HandleElecOptionAsync();
                    break;
                }
            case "gas":
                {  
                    await _chooseMenu.HandleGasOptionAsync();
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
            case "return":
                {
                    await _chooseMenu.HandleReturnAsync();
                    break;
                }
        }
    }

}