using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUtils;

public class HitFire : SkillBase
{
    protected override void Awake()
    {
        base.Awake();
        skillCode = (int)SkillEnum.HitFire;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(SkillManager.inst.GetSkillData(skillCode).destroyTime);
        transform.gameObject.SetActive(false);
    }

    public override void UseSkill(Vector2 target, int playerDamage)
    {
        base.UseSkill(target, playerDamage);
        SoundManager.inst.PlaySound(SoundType.Skill, (int)SkillSound.Skill_1_HitFire);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if(enemy != null) 
        {
            enemy.Attacked((int)Math.Floor(playerDamage * SkillManager.inst.GetSkillData(skillCode).skillValue));
        }       
    }  
}
