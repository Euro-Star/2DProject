using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager inst { get { return instance; } }
    
    [SerializeField] private GameObject[] prefabsEnemy;
    [SerializeField] private GameObject prefabsDefalutAttack;
    [SerializeField] private GameObject prefabsMoney;
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private GameObject prefabEnemySpawnPoint;
    [SerializeField] private GameObject prefabDefaultSpawnPoint;

    private Transform defaultSpawnPosition;
    private Transform enemySpawnPosition;

    private List<GameObject> pools_Enemy;
    private List<GameObject> pools_DefalutAttack;
    private List<GameObject> pools_Money;
    private List<DefaultAttackComponent> defaultAttackComponents;
    private List<List<GameObject>> pools_Skill;

    private GameObject gameObj;
    private float regenTime = 7f;
    private float deltaTime = 0f;
    private Vector3 defaultPosition;
    private Quaternion defaultQuat;

    private void Awake()
    {
        pools_Enemy = new List<GameObject>();
        pools_DefalutAttack = new List<GameObject>();
        defaultAttackComponents = new List<DefaultAttackComponent>();
        pools_Skill = new List<List<GameObject>>();
        pools_Money = new List<GameObject>();

        defaultPosition = new Vector3(100, 100, 100);
        defaultQuat = new Quaternion();

        instance = this;
    }

    private void Start()
    {
        enemySpawnPosition = Instantiate(prefabEnemySpawnPoint, defaultPosition, defaultQuat).GetComponent<Transform>();
        defaultSpawnPosition = Instantiate(prefabDefaultSpawnPoint, defaultPosition, defaultQuat).GetComponent<Transform>();

        AttackObjSpawn(30);
        SkillSpawn();
        SpawnMoney();

        SceneManager.sceneLoaded += LoadSceneEvent;
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
    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().name == "LoadingScene" || SceneManager.GetActiveScene().name == "Stage_5")
        {
            return;
        }

        enemySpawnPosition = Instantiate(prefabEnemySpawnPoint, defaultPosition, defaultQuat).GetComponent<Transform>();

        pools_Enemy.Clear();

        EnemySpawn(0, 10);
        PoolEnemy();
    }

    void EnemySpawn(int stage, int num)
    {
        // 적 스폰
        for(int i = 0; i < num; i++) 
        {         
            gameObj = Instantiate(prefabsEnemy[stage], enemySpawnPosition);
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
        for (int i = 0; i < SkillManager.inst.SkillLength(); i++)
        {
            List<GameObject> list = new List<GameObject>();

            for (int j = 0; j < 4; j++)
            {
                if (SkillManager.inst.GetSkillType(i) == "Heal" || SkillManager.inst.GetSkillType(i) == "Buff")
                {
                    gameObj = Instantiate(SkillManager.inst.GetSkillPrefab(i), Player.player.transform);
                    list.Add(gameObj);
                    gameObj.SetActive(false);
                }
                else
                {
                    gameObj = Instantiate(SkillManager.inst.GetSkillPrefab(i), defaultSpawnPosition);
                    list.Add(gameObj);
                    gameObj.SetActive(false);
                }
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
            if(p != null)
            {
                if (p.activeSelf == false)
                {
                    p.SetActive(true);
                    p.transform.position = spawnPosition[Random.Range(0, spawnPosition.Length - 1)].position;
                }
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
            if (p != null)
            {
                if (p.activeSelf == true)
                {
                    return true;
                }
            }               
        }

        return false;
    }

    public List<GameObject> GetEnemys()
    {
        return pools_Enemy;
    }
}
