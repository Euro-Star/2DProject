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
    private int totalExp;

    private Dictionary<int, LevelData> dictLevelData;

    public event EventHandler ExpChangeEvent;
    public event EventHandler LevelChangeEvent;


    public int GetLevel() { return level; }
    public int GetExp() { return exp; }
    public int GetAtk() { return atk; }

    public void InitPlayerData(PlayerData playerData)
    {
        atk = playerData.atk;
        level = playerData.level;
        exp = playerData.exp;
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
        ExpChangeEvent?.Invoke(this, EventArgs.Empty);
    }
}
