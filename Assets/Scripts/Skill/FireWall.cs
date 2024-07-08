using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUtils;

public class FireWall : SkillBase
{
    int SkillCode = 1;
    float CurrentTime = 0f;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        CurrentTime += Time.deltaTime;

        if(CurrentTime >= skillData.damageOverTime)
        {
            if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy)
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                enemy.Attacked((int)Math.Floor(playerDamage * skillData.damageRatio));
                enemy.KnockBack();
            }

            CurrentTime = 0f;
        }   
    }
}
