using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using System.Threading;
using System.IO;
using static AbilityComponent;

public class AbilityComponent : MonoBehaviour
{
    private int atk;
    private int level;
    private int exp;
    private int statPoint;

    private Dictionary<int, LevelData> dictLevelData;

    public event EventHandler ExpChangeEvent;
    public event EventHandler LevelChangeEvent;


    public int GetLevel() { return level; }
    public int GetExp() { return exp; }
    public int GetTotalExp() { return dictLevelData[level].totalExp; }
    public int GetStatPoint() { return statPoint; }
    public int GetMaxHp() { return Player.player.healthComponent.GetMaxHp(); }
    public int GetHp() { return Player.player.healthComponent.GetHp(); }
    public int GetStatAtk() { return atk; }
    public int GetAtk()
    {
        int res = UnityEngine.Random.Range(atk - atk / 2, atk + atk / 2);
        if (res <= 1)
        {
            return 1;
        }
        else
        {
            return res;
        }
    }

    public void InitPlayerData(PlayerData playerData)
    {
        atk = playerData.atk;
        level = playerData.level;
        exp = playerData.exp;

        ExpChangeEvent?.Invoke(this, EventArgs.Empty);
        LevelChangeEvent?.Invoke(this, EventArgs.Empty);
    }
    
    public PlayerData GetPlayerData()
    {
        PlayerData playerData = new PlayerData();

        playerData.atk = atk;
        playerData.level = level;
        playerData.exp = exp;

        return playerData;
    }

    private void Awake()
    {
        dictLevelData = Utils.JsonToDictionary<int, LevelData>("LevelData");
    }

    public void AddExp(int exp)
    {
        this.exp += exp;

        if(this.exp >= GetTotalExp() && GetTotalExp() != 0)
        {
            LevelUp();
        }

        ExpChangeEvent?.Invoke(this, EventArgs.Empty);

        ServerManager.inst.UpdateExp(this.exp);
    }

    public void LevelUp()
    {
        exp %= GetTotalExp();
        statPoint += 2;
        ++level;
        Player.player.healthComponent.MaxHeal();

        LevelChangeEvent?.Invoke(this, EventArgs.Empty);

        ServerManager.inst.UpdateExp(exp);
        ServerManager.inst.UpdateLevel(level);
        ServerManager.inst.UpdateStatPoint(statPoint);
    }

    public void IncreaseAtk()
    {

        Debug.Log(statPoint);

        if(statPoint > 0)
        {
            --statPoint;
            ++atk;

            ServerManager.inst.UpdateAtk(atk);
            ServerManager.inst.UpdateStatPoint(statPoint);
        }
    }

    public void IncreaseHp()
    {
        Debug.Log(statPoint);

        if (statPoint > 0)
        {
            --statPoint;

            Player.player.healthComponent.IncreaseHp();
            ServerManager.inst.UpdateHp(GetMaxHp());
            ServerManager.inst.UpdateStatPoint(statPoint);
        }
    }
}
