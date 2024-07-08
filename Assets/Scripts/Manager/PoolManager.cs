using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    /* Skill_Pool */

    List<List<GameObject>> pools_Skill;

    /////////////////
    
    [SerializeField]
    GameObject[] prefabsEnemy;
    [SerializeField]
    GameObject prefabsDefalutAttack;
    [SerializeField]
    GameObject prefabsMoney;
    [SerializeField]
    Transform[] spawnPosition;
    [SerializeField]
    Transform defaultSpawnPosition;
    
    protected List<GameObject> pools_Enemy;
    protected List<GameObject> pools_DefalutAttack;
    protected List<GameObject> pools_Money;
    protected List<DefaultAttackComponent> defaultAttackComponents;

    protected GameObject gameObj;
    protected float regenTime = 7f;
    protected float deltaTime = 0f;

    public static PoolManager inst;

    private void Awake()
    {
        pools_Enemy = new List<GameObject>();
        pools_DefalutAttack = new List<GameObject>();
        defaultAttackComponents= new List<DefaultAttackComponent>();
        pools_Skill = new List<List<GameObject>>();
        pools_Money = new List<GameObject>();

        inst = this;
    }

    private void Start()
    {
        EnemySpawn(0, 10);
        AttackObjSpawn(30);
        SkillSpawn();
        PoolEnemy();
        SpawnMoney();
    }

    private void Update()
    {
        deltaTime += Time.deltaTime;

        if(deltaTime >= regenTime)
        {
            PoolEnemy();
            deltaTime = 0f;
        }
    }

    void EnemySpawn(int stage, int num)
    {
        // 적 스폰
        for(int i = 0; i < num; i++) 
        {         
            gameObj = Instantiate(prefabsEnemy[stage], defaultSpawnPosition);
            pools_Enemy.Add(gameObj);

            gameObj.SetActive(false);
        }
    }

    void AttackObjSpawn(int num)
    {
        // 기본공격 총알 스폰
        for (int i = 0; i < num; i++)
        {
            gameObj = Instantiate(prefabsDefalutAttack, defaultSpawnPosition);
            pools_DefalutAttack.Add(gameObj);
            defaultAttackComponents.Add(gameObj.GetComponent<DefaultAttackComponent>());

            gameObj.SetActive(false);
        }
    }

    public void SkillSpawn()
    {
        // 스킬 오브젝트 스폰
        for (int i = 0; i < SkillManager.instance.SkillLength(); i++)
        {
            List<GameObject> list = new List<GameObject>();

            for (int j = 0; j < 3; j++)
            {
                gameObj = Instantiate(SkillManager.instance.GetSkillPrefab(i), defaultSpawnPosition);
                list.Add(gameObj);
                gameObj.SetActive(false);
            }

            pools_Skill.Add(list);
        }
    }

    public void SpawnMoney()
    {
        for (int i = 0; i < 5; i++)
        {
            gameObj = Instantiate(prefabsMoney, defaultSpawnPosition);
            pools_Money.Add(gameObj);

            gameObj.SetActive(false);
        }
    }


    void PoolEnemy()
    {
        foreach(GameObject p in pools_Enemy)
        {
            if(p.activeSelf == false)
            {
                p.SetActive(true); 
                p.transform.position = spawnPosition[Random.Range(0, spawnPosition.Length - 1)].position;
            }
        }
    }

    public void PoolDefaultAttack(Vector2 spawnPosition, Vector2 targetPosition)
    {
        for(int i = 0; i < pools_DefalutAttack.Count; ++i)
        {
            if (pools_DefalutAttack[i].activeSelf == false)
            {
                pools_DefalutAttack[i].SetActive(true);
                pools_DefalutAttack[i].transform.position = spawnPosition;

                defaultAttackComponents[i].targetVec = targetPosition;

                break;
            }
        }
    }

    public GameObject PoolSkill(int skillIndex)
    {
        foreach (GameObject p in pools_Skill[skillIndex])
        {
            if (p.activeSelf == false)
            {
                p.SetActive(true);
                return p;
            }
        }

        return null;
    }

    public GameObject PoolMoney()
    {
        foreach (GameObject p in pools_Money)
        {
            if (p.activeSelf == false)
            {
                p.SetActive(true);
                return p;
            }
        }

        gameObj = Instantiate(prefabsMoney, defaultSpawnPosition);
        pools_Money.Add(gameObj);

        return gameObj;
    }

    public bool IsActiveEnemy()
    {
        foreach (GameObject p in pools_Enemy)
        {
            if (p.activeSelf == true)
            {
                return true;
            }
        }

        return false;
    }

    public List<GameObject> GetEnemys()
    {
        return pools_Enemy;
    }
}
