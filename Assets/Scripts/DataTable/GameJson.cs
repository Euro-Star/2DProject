using System;

[Serializable]
public class SkillData
{
    public int skillCode;
    public string skillName;
    public int skillLevel;
    public float damageOverTime;
    public float destroyTime;
    public float delay;
    public float coolTime;
    public float damageRatio;
    public int numFloating;
}

[Serializable]
public class PlayerData
{
    public int atk;
    public int hp;
    public int level;
    public int exp;
    public int money;
}

[Serializable]
public class LevelData
{
    public int level;
    public int totalExp;
}

