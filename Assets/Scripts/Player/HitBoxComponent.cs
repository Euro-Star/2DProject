using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using System;
using Unity.VisualScripting;

public class HitBoxComponent : MonoBehaviour
{
    [SerializeField]
    HealthComponent healthComponent;

    private bool bHitCooltime;

    private void Start()
    {
        healthComponent.DeathEvent += Death;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy || Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Boss)
        {
            if(!bHitCooltime)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                healthComponent.HitDamage(enemy.GetAtk());
                bHitCooltime = true;
                StartCoroutine(HitCooltime());
            }         
        }
    }

    private void Death(object sender, EventArgs eventArgs)
    {
        Debug.Log("Die");
    }

    private IEnumerator HitCooltime()
    {   
        yield return new WaitForSeconds(0.8f);
        bHitCooltime = false;
    }

    public HealthComponent GetHealthComponent()
    {
        return healthComponent;
    }
}
