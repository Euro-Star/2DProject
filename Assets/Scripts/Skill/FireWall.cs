using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUtils;

public class FireWall : SkillBase
{
    float currentTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        skillCode = (int)SkillEnum.FireWall;
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
        currentTime += Time.deltaTime;

        if(currentTime >= SkillManager.inst.GetSkillData(skillCode).damageOverTime)
        {
            if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy)
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                enemy.Attacked((int)Math.Floor(playerDamage * SkillManager.inst.GetSkillData(skillCode).skillValue));
                enemy.KnockBack();
            }

            currentTime = 0f;
        }   
    }
}
