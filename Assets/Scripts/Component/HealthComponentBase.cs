using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor.Build.Pipeline;

public interface IHealth
{
    public void InitHealth(int hp);
    public void HitDamage(int hp);
}


public class HealthComponentBase : MonoBehaviour, IHealth
{
    [SerializeField]
    GameObject textDamagePrefab;

    protected int maxHp;
    protected int currnetHp;

    public event EventHandler DeathEvent;
    public event EventHandler HpChangeEvent;

    public int GetMaxHp() { return maxHp; }
    public int GetCurrnetHp() { return currnetHp; }

    protected void HpChageEventInvoke() { HpChangeEvent?.Invoke(this, EventArgs.Empty); }
    protected void DeathEventInvoke() { DeathEvent?.Invoke(this, EventArgs.Empty); }

    public void InitHealth(int hp)
    {
        maxHp = hp;
        currnetHp = hp;

        HpChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public virtual void HitDamage(int hp)
    {
        GameObject uiObj = Instantiate<GameObject>(textDamagePrefab);
        FloatingUI damageUI = uiObj.GetComponent<FloatingUI>();

        uiObj.transform.position = transform.position;
        damageUI.SetTextDamage(hp);

        currnetHp -= hp;

        if (currnetHp <= 0)
        {
            currnetHp = 0;
            DeathEventInvoke();
        }

        HpChageEventInvoke();
    }
}
