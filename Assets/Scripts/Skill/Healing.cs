using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : SkillBase
{
    protected override void Awake()
    {
        base.Awake();
        skillCode = (int)SkillEnum.Healing;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void UseSkill(Vector2 target, int playerDamage)
    {
        base.UseSkill(target, playerDamage);

        Player.player.healthComponent.Heal(SkillManager.inst.GetSkillData(skillCode).amountOfHeal);
    }
}
