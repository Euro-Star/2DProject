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

    private int maxHealth;
    private int currnetHealth;

    public event EventHandler DeathEvent;

    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return currnetHealth; }
    public void InitHealth(int Hp)
    {
        maxHealth = Hp;
        currnetHealth = Hp;
    }

    public void HitDamage(int Hp)
    {
        GameObject uiObj = Instantiate<GameObject>(textDamagePrefab);
        FloatingUI damageUI = uiObj.GetComponent<FloatingUI>();

        uiObj.transform.position = transform.position;
        damageUI.SetTextDamage(Hp);

        currnetHealth -= Hp;

        if (currnetHealth <= 0)
        {
            DeathEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
