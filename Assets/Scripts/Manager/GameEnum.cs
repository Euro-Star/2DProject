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
    Wall,
    EnemyProjectile,
    PlayerHitBox,
    Boss
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
    StageUI,
    DeathUI,
    GameTestUI
}

public enum SkillEnum
{
    HitFire = 0,
    FireWall,
    Healing,
    SpeedUp
}

public enum CharacterAnim
{
    Idle = 0,
    Run,
    Death,
    Stun,
    AttackSword,
    AttackBow,
    AttackMagic,
    SkillSword,
    SkillBow,
    SkillMagic
}

public enum SoundType
{
    Bgm,
    Skill,
    Enemy
}

public enum BgmSound
{
    Bgm_Start,
    Bgm_Stage,
}

public enum SkillSound
{
    Skill_1_HitFire,
    Skill_2_FireWall,
    Skill_3_Healing,
    Skill_4_SpeedUp,
}

public enum EnemySound
{
    SlimeHit,
    SlimeDeath_1,
    SlimeDeath_2,
    SlimeBoss_HitSkill,
    SlimeBoss_Skill_1
}





