using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using Unity.VisualScripting;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] skillPrefabs;

    private static SkillManager instance;
    public static SkillManager inst { get { return instance; } }

    [Tooltip("스킬레벨에 대한 정보 저장")]
    private List<Dictionary<int, SkillData>> skillData;
    private int[] skillLevel;


    private void Awake()
    {
        instance = this;
        skillData = new List<Dictionary<int, SkillData>>();

        for(int i = 0; i< skillPrefabs.Length; i++) 
        {
            skillData.Add(Utils.JsonToDictionary<int, SkillData>(SkillIndexToName(i)));
        }
    }

    private string SkillIndexToName(int index)
    {
        switch(index) 
        {
            case 0:
                return "SkillData_0_HitFire";
            case 1:
                return "SkillData_1_FireWall";
            case 2:
                return "SkillData_2_Healing";
            case 3:
                return "SkillData_3_SpeedUp";
            default:
                return null;
        }
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
        return skillData[index][skillLevel[index]];
    }

    public int GetSkillLevel(int skillCode)
    {
        return skillLevel[skillCode];
    }

    public void SkillLevelUp(int skillCode)
    {
        ++skillLevel[skillCode]; 
    }

    public void InitSkillLevel(int[] skillLevel)
    {
        this.skillLevel = skillLevel;
    }

    public int SkillLength()
    {
        return skillPrefabs.Length;
    }
}
