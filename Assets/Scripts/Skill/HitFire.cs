using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUtils;

public class HitFire : SkillBase
{
    int SkillCode = 0;

    protected override void Awake()
    {
        base.Awake();
        skillData = SkillManager.instance.GetSkillData(SkillCode);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void UseSkill(Vector2 target, int playerDamage)
    {
        base.UseSkill(target, playerDamage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            enemy.Attacked((int)Math.Floor(playerDamage * skillData.damageRatio));
            enemy.KnockBack();
        }
    }  
}
