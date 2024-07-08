using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] skillPrefabs;

    public static SkillManager instance;

    private SkillData[] skillData;


    private void Awake()
    {
        instance = this;
        skillData = Utils.JsonDataArrayParse<SkillData>("SkillData");
        Debug.Log(skillData[0].skillName);
    }

    public SkillBase GetSkill(int index)
    {
        return skillPrefabs[index].GetComponent<SkillBase>();
    }

    public GameObject GetSkillPrefab(int index)
    {
        return skillPrefabs[index];
    }

    public SkillData GetSkillData(int index)
    {
        return skillData[index];
    }

    //public void AddSkillInfo(SkillInfo skillInfo)
    //{
    //    skillInfoList.Add(skillInfo);
    //}

    public int SkillLength()
    {
        return skillPrefabs.Length;
    }
}
