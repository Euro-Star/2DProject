using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    enum SlimeAnim
    {
        Idle,
        Hurt,
        Death
    }

    enum SlimeType
    {
        GreenSlime, 
        BlueSlime,
        RedSlime
    }

    // GreenSlime = 0, BlueSlime = 0.5, RedSlime = 1;
    public float SlimeAnimState;

    private Animator anim;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        InitSlime((int)UnityEngine.Random.Range(0, 3));
    }

    protected override void Start()
    {
        base.Start();
        healthComponent.AttackedEvent += Hurt;
        healthComponent.DeathEvent += DeathAnim;
    }

    private void LateUpdate()
    {
        SlimeAnimation((int)SlimeAnim.Idle);
    }

    private void InitSlime(int type)
    {
        switch(type)
        {
            case 0:
                {
                    SlimeAnimState = 0f;
                    break;
                }
            case 1:
                {
                    SlimeAnimState = 0.5f; 
                    break;
                }
            case 2: 
                {
                    SlimeAnimState = 1f;
                    break;
                }
        }
    }

    public void SlimeAnimation(int num)
    {
        switch (num)
        {
            case 0: //Idle
                anim.SetFloat("IdleState", SlimeAnimState);
                break;

            case 1: //Hurt
                anim.SetTrigger("isHurt");
                anim.SetFloat("HurtState", SlimeAnimState);
                break;

            case 2: //Death
                anim.SetTrigger("isDeath");
                anim.SetFloat("DeathState", SlimeAnimState);
                break;
        }
    }

    private void Hurt(object sender, EventArgs eventArgs)
    {
        anim.Rebind();
        SlimeAnimation((int)SlimeAnim.Hurt);
        SoundManager.inst.PlaySound(SoundType.Enemy, (int)EnemySound.SlimeHit);
    }

    private void DeathAnim(object sender, EventArgs eventArgs)
    {
        int randSound = UnityEngine.Random.Range((int)EnemySound.SlimeDeath_1, (int)EnemySound.SlimeDeath_2 + 1);
        SlimeAnimation((int)SlimeAnim.Death);
        SoundManager.inst.PlaySound(SoundType.Enemy, randSound);
    }
}
