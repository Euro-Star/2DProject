using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int money = 0;

    public event EventHandler moneyChangeEvent;

    public void InitInventory(PlayerData playerData)
    {
        money = playerData.money;
        moneyChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void AddMoney(int money)
    {
        this.money += money;
        moneyChangeEvent?.Invoke(this, EventArgs.Empty);

        ServerManager.inst.UpdateMoney(this.money);
    }

    public void UseMoney(int money)
    {
        this.money -= money;
        moneyChangeEvent?.Invoke(this, EventArgs.Empty);

        ServerManager.inst.UpdateMoney(this.money);
    }

    public int GetMoney()
    {
        return money;
    }
}
