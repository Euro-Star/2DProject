using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using GameUtils;
using UnityEngine.SceneManagement;

public class HealthComponentPlayer : HealthComponentBase
{
    public int GetMaxHp() { return maxHp; }
    public int GetHp() { return currnetHp; }

    private void Start()
    {
        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        currnetHp = maxHp;

        HpChageEventInvoke();
    }

    public void Heal(int heal) 
    {
        currnetHp += heal;

        if(currnetHp > maxHp)
        {
            currnetHp = maxHp;
        }

        HpChageEventInvoke();
    }

    public void MaxHeal()
    {
        currnetHp = maxHp;

        HpChageEventInvoke();
    }

    public void IncreaseHp()
    {
        maxHp += 10;
        currnetHp += 10;

        HpChageEventInvoke();
    }

    //public void BossHPView()
    //{
    //    if (Utils.StringToEnum<GameTag>(this.gameObject.tag) == GameTag.Boss)
    //    {
    //        HUD.inst.UpdateBossHP(maxHp, currnetHp);
    //    }
    //}
}
