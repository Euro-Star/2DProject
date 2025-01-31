using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthComponentEnemy : HealthComponentBase
{
    //Enemy ���� HealthComponent Ȯ�� �� �߰�

    public event EventHandler AttackedEvent;

    public override void HitDamage(int hp)
    {
        base.HitDamage(hp);
        AttackedEvent?.Invoke(this, EventArgs.Empty);
    }
}
