using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Collections.Generic;

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
    public int statPoint;
    public int[] skillLevel;

    public PlayerData() 
    {
        atk = 1;
        hp = 10;
        level = 1;
        exp = 0;
        money = 0;
        statPoint = 0;

        skillLevel = Enumerable.Repeat<int>(1, 4).ToArray<int>();
    }
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
