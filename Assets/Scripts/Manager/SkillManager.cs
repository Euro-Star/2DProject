using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using Unity.VisualScripting;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject[] skillPrefabs;
    [SerializeField] private Sprite[] skillImages;
    [SerializeField] private Color[] skillImageColors;

    private static SkillManager instance;
    public static SkillManager inst { get { return instance; } }

    [Tooltip("스킬레벨에 대한 정보 저장")]
    private List<Dictionary<int, SkillData>> skillData;
    private int[] skillLevel;


    private void Awake()
    {
        instance = this;
        skillData = new List<Dictionary<int, SkillData>>();
        skillLevel = Enumerable.Range(1, skillPrefabs.Length).ToArray();

        for (int i = 0; i< skillPrefabs.Length; i++) 
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

    public string GetSkillType(int index)
    {
        return skillData[index][skillLevel[index]].skillType;
    }

    public int GetSkillLevel(int index)
    {
        return skillLevel[index];
    }

    public Sprite GetSkillImage(int index)
    {
        return skillImages[index];
    }

    public Color GetSkillImageColor(int index)
    {
        return skillImageColors[index];
    }

    public void SkillLevelUp(int index)
    {
        ++skillLevel[index];
        
        ServerManager.inst.UpdateSkillLevel(index, skillLevel[index]);
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
