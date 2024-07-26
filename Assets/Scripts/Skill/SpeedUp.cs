using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameUtils;

public class SpeedUp : SkillBase
{
    protected override void Awake()
    {
        base.Awake();
        skillCode = (int)SkillEnum.SpeedUp;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(SkillManager.inst.GetSkillData(skillCode).destroyTime);

        Player.player.ReturnAttackSpeed();
        transform.gameObject.SetActive(false);
    }

    public override void UseSkill(Vector2 target, int playerDamage)
    {
        base.UseSkill(target, playerDamage);

        Player.player.AttackSpeedUp(SkillManager.inst.GetSkillData(skillCode).skillValue);
    }
}
