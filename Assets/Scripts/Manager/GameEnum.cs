using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTag
{   
    Untagged = 0,
    EditorOnly,
    Player,
    Manager,
    UI,
    Enemy,
    Wall
}

public enum EnemyStatus
{
    Default = 0,
    KnockBack
}

public enum GameUI
{
    StatUI = 0,
    SkillUI,
}

public enum SkillEnum
{
    HitFire = 0,
    FireWall,
    Healing,
    SpeedUp
}





