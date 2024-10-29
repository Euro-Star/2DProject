using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

[Serializable]
public class SkillData
{
    public string skillName;
    public float damageOverTime;
    public float destroyTime;
    public float delay;
    public float coolTime;
    public float skillValue;
    public int amountOfHeal;
    public string skillExplane;
    public int needMoney;
    public string skillType;
}

[Serializable]
public class PlayerData
{
    public int atk;
    public int hp;
    public int level;
    public int exp;
    public int money;
    public int[] skillLevel;
}

[Serializable]
public class LevelData
{
    public int level;
    public int totalExp;
}

[Serializable]
public class StageData
{
    public int hp;
    public int atk;
    public int exp;
    public int money;
    public int openLevel;
}
