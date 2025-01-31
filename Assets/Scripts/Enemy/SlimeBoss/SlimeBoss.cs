using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using GameUtils;
using UnityEngine.SceneManagement;

public class SlimeBoss : Boss
{
    [SerializeField] private GameObject[] skill_Prefabs;
    [SerializeField] private GameObject warningSign_Prefab;
    [SerializeField] private GameObject warningSignSquare_Prefab;

    private Queue<Vector3>[] targetPositions;
    private Animator anim;
    private float t;
    private int[] arr;

    private Queue<float> skill_3_Angle;
    private float skill_3_AngleOffset = 90f;
    private float patternCycle = 2.5f;

    protected override void Awake()
    {
        base.Awake();

        stageData = Utils.JsonToDictionary<string, StageData>("StageData");
        targetPositions = new Queue<Vector3>[3];
        anim = GetComponent<Animator>();
        
        arr = new int[3];
        skill_3_Angle = new Queue<float>();

        for (int i = 0;i < targetPositions.Length; ++i)
        {
            targetPositions[i] = new Queue<Vector3>();
        }

    }

    protected override void Start()
    {
        base.Start();

        healthComponent.AttackedEvent += Hurt;
        healthComponent.DeathEvent += PlayingForThk;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if(t >= patternCycle)
        {
            UseRandomSkill();
            t = 0f;
        }
    }

    private void UseRandomSkill()
    { 
        int num = UnityEngine.Random.Range(0, 3);

        switch (num)
        {
            case 0:
                Skill_1();
                break;
            case 1:
                Skill_2();
                break;
            case 2: 
                Skill_3();
                break;
            default:
                break;
        }

    }

    private void Skill_1()
    {
        Vector3 target = Player.player.transform.position;
        WarningSign sign = Instantiate(warningSign_Prefab, target, Quaternion.identity).GetComponent<WarningSign>();

        targetPositions[0].Enqueue(target);
        anim.SetTrigger("magic");
        sign.WarningCircle(10f, target, 3f);
        sign.warningCompEvent += SpawnSkill_1;
    }

    private void Skill_2()
    {
        anim.SetTrigger("book");
        StartCoroutine(Skill_2_Maintain());
    }

    private void Skill_3()
    {
        anim.SetTrigger("magic");

        for (int i = -20; i <= 20; i+=10)
        {
            Vector3 target = new Vector3(8f, 0f, 0f);
            WarningSignSquare sign = Instantiate(warningSignSquare_Prefab, target, Quaternion.Euler(0f, 0f, i)).GetComponent<WarningSignSquare>();
            targetPositions[2].Enqueue(target);
            skill_3_Angle.Enqueue(i + skill_3_AngleOffset);
            sign.WarningSquare(1f, 2f);
            sign.warningCompEvent += SpawnSkill_3;
        }
    }

    private void Sign_warningCompEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private IEnumerator Skill_2_Maintain()
    {
        float skill_2_DeltaTime = 0f;
        int num = 0;

        while(num < 10)
        {
            skill_2_DeltaTime += Time.deltaTime;

            if(skill_2_DeltaTime > 0.15f)
            {
                Vector3 target = Player.player.transform.position;
                WarningSign sign = Instantiate(warningSign_Prefab, target, Quaternion.identity).GetComponent<WarningSign>();

                targetPositions[1].Enqueue(target);
                sign.WarningCircle(5f, target, 2f);
                sign.warningCompEvent += SpawnSkill_2;

                ++num;
                skill_2_DeltaTime = 0f;
            }

            yield return null;
        }
    }

    private void SpawnSkill_1(object sender, EventArgs eventArgs)
    {
        Instantiate(skill_Prefabs[0], targetPositions[0].Dequeue(), Quaternion.identity);
        SoundManager.inst.PlaySound(SoundType.Enemy, (int)EnemySound.SlimeBoss_Skill_1);
    }

    private void SpawnSkill_2(object sender, EventArgs eventArgs)
    {
        Instantiate(skill_Prefabs[1], targetPositions[1].Dequeue(), Quaternion.identity);
    }

    private void SpawnSkill_3(object sender, EventArgs eventArgs)
    {
        float angle = skill_3_Angle.Dequeue();
        SlimeBossSkill_3 skill_3 = Instantiate(skill_Prefabs[2], targetPositions[2].Dequeue(), Quaternion.Euler(0f, 0f, angle)).GetComponent<SlimeBossSkill_3>();

        skill_3.SetParticleRotation(angle);
    }

    private void Hurt(object sender, EventArgs eventArgs)
    {
        SoundManager.inst.PlaySound(SoundType.Enemy, (int)EnemySound.SlimeHit);
    }

    private void PlayingForThk(object sender, EventArgs eventArgs)
    {
        UIManager.inst.UIController(GameUI.GameTestUI, true);
    }
}
