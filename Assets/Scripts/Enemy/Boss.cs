using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    public override void Attacked(int damage)
    {
        base.Attacked(damage);
        HUD.inst.UpdateBossHP(healthComponent.GetMaxHp(), healthComponent.GetCurrnetHp());
    }
}
