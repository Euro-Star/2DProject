using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameUtils;

public interface IEnemy
{
    public int GetAtk();
    public void Attacked(int damage);
}


public class EnemyBase : MonoBehaviour, IEnemy
{
    protected Rigidbody2D targetRigid;
    protected Rigidbody2D rigid;
    protected ItemDropComponent itemDropComponent;

    public HealthComponentEnemy healthComponent;

    protected int exp = 50;
    protected int atk = 1;
    protected int money = 10;

    protected Vector2 dirVec;
    protected Vector2 nextVec;
    protected EnemyStatus enemyStatus = EnemyStatus.Default;

    protected Dictionary<string, StageData> stageData;
    protected StageData data;


    protected virtual void Awake()
    {
        stageData = Utils.JsonToDictionary<string, StageData>("StageData");
        rigid = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponentEnemy>();
        itemDropComponent = GetComponent<ItemDropComponent>();        

        Init();
    }
    protected virtual void Start()
    {
        healthComponent.DeathEvent += Death;
    }

    protected virtual void OnEnable()
    {
        targetRigid = Player.player.GetComponent<Rigidbody2D>();
        healthComponent.InitHealth(data.hp);
        enemyStatus = EnemyStatus.Default;
    }

    protected void OnDisable()
    {
        StopCoroutine(Reset());
    }

    protected void Init()
    {
        data = stageData[SceneManager.GetActiveScene().name];

        healthComponent.InitHealth(data.hp);
        exp = data.exp;
        atk = data.atk;
        money = data.money;      
    }

    public int GetAtk()
    {
        int res = UnityEngine.Random.Range(atk - atk / 2, atk + atk / 2);
        if (res <= 1)
        {
            return 1;
        }
        else
        {
            return res;
        }
    }

    public virtual void Attacked(int damage)
    {
        healthComponent.HitDamage(damage);
    }

    protected IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.15f);
        enemyStatus = EnemyStatus.Default;
    }

    protected void Death(object sender, EventArgs eventArgs)
    {
        itemDropComponent.DropMoney(transform.position, money);
        itemDropComponent.DropExp(exp);
        transform.gameObject.SetActive(false);
    }
}
