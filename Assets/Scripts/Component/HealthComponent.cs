using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    GameObject textDamagePrefab;

    private int maxHp;
    private int currnetHp;

    public event EventHandler DeathEvent;
    public event EventHandler HpChangeEvent;

    public int GetMaxHp() { return maxHp; }
    public int GetHp() { return currnetHp; }
    public void InitHealth(int Hp)
    {
        maxHp = Hp;
        currnetHp = Hp;

        HpChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void HitDamage(int Hp)
    {
        GameObject uiObj = Instantiate<GameObject>(textDamagePrefab);
        FloatingUI damageUI = uiObj.GetComponent<FloatingUI>();

        uiObj.transform.position = transform.position;
        damageUI.SetTextDamage(Hp);

        currnetHp -= Hp;

        if (currnetHp <= 0)
        {
            currnetHp = 0;
            DeathEvent?.Invoke(this, EventArgs.Empty);
        }

        HpChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int heal)
    {
        currnetHp += heal;

        if(currnetHp > maxHp)
        {
            currnetHp = maxHp;
        }

        HpChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void MaxHeal()
    {
        currnetHp = maxHp;

        HpChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseHp()
    {
        maxHp += 10;
        currnetHp += 10;

        HpChangeEvent?.Invoke(this, EventArgs.Empty);
    }
}
