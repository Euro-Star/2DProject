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

    private HealthComponent healthComponent;

    public event EventHandler ExpChangeEvent;
    public event EventHandler LevelChangeEvent;


    public int GetLevel() { return level; }
    public int GetExp() { return exp; }
    public int GetAtk() { return atk; }
    public int GetTotalExp() { return dictLevelData[level].totalExp; }
    public int GetStatPoint() { return statPoint; }
    public int GetMaxHp() { return healthComponent.GetMaxHp(); }
    public int GetHp() { return healthComponent.GetHp(); }
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

    private void OnEnable()
    {
        healthComponent = Player.player.healthComponent;
    }

    public void AddExp(int exp)
    {
        this.exp += exp;

        if(this.exp >= GetTotalExp() && GetTotalExp() != 0)
        {
            LevelUp();
        }

        ExpChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void LevelUp()
    {
        exp %= GetTotalExp();
        statPoint += 2;
        ++level;
        healthComponent.MaxHeal();

        LevelChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseAtk()
    {
        if(statPoint > 0)
        {
            --statPoint;
            ++atk;
        }
    }

    public void IncreaseHp()
    {
        if(statPoint > 0)
        {
            --statPoint;
            healthComponent.IncreaseHp();
        }
    }
}
