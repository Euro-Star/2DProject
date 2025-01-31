using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthComponentEnemy : HealthComponentBase
{
    //Enemy 전용 HealthComponent 확장 시 추가

    public event EventHandler AttackedEvent;

    public override void HitDamage(int hp)
    {
        base.HitDamage(hp);
        AttackedEvent?.Invoke(this, EventArgs.Empty);
    }
}
