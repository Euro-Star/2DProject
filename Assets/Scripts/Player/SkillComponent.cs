
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    private int[] useSkillIndex = { 0, 1 };
    private float[] skillCoolTimes;
    private bool[] bAvailable;

    private SkillManager skillManager;
    private AbilityComponent abilityComponent;

    private void Awake()
    {
        skillCoolTimes = Enumerable.Repeat(0f, 4).ToArray();
        bAvailable = Enumerable.Repeat(false, 4).ToArray();
    }

    private void Start()
    {
        skillManager = SkillManager.inst;
        abilityComponent = Player.player.abilityComponent;
    }

    public bool[] IsSkillsAvailable() { return bAvailable; }
    public float GetCoolTime(int index) { return skillCoolTimes[index]; }


    private IEnumerator CoolTime(int index)
    {
        while (skillCoolTimes[index] > 0f)
        {
            skillCoolTimes[index] -= Time.deltaTime;
            HUD.instance.dele_UpdateCooltime(index, skillCoolTimes[index]);
            yield return new WaitForFixedUpdate();
        }

        bAvailable[index] = false;
    }


    public void Skill(int index)
    {
        if (!bAvailable[index])
        {
            SkillBase skill = PoolManager.inst.PoolSkill(useSkillIndex[index]).GetComponent<SkillBase>();
            skill.UseSkill(Player.player.targetPos, abilityComponent.GetAtk());
            bAvailable[index] = true;

            skillCoolTimes[index] = SkillManager.inst.GetSkillData(useSkillIndex[index]).coolTime;
            StartCoroutine(CoolTime(index));
        }
        else
        {
            Debug.Log("Skill Cooltime : " + skillCoolTimes[index]);
        }   
    }
}
