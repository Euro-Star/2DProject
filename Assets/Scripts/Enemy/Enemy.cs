using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameUtils;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D targetRigid;
    [SerializeField] private float speed;
    [SerializeField] private float knockBackForce;

    private Rigidbody2D rigid;
    private ItemDropComponent itemDropComponent;

    private int exp = 50;
    private int atk = 1;
    private int money = 10;

    private Vector2 dirVec;
    private Vector2 nextVec;
    private EnemyStatus enemyStatus = EnemyStatus.Default;
    
    protected Dictionary<string, StageData> stageData;
    protected StageData data;

    public EventHandler attackedEvent;
    public HealthComponent healthComponent;

    public int GetAtk()
    {
        int res = UnityEngine.Random.Range(atk - atk / 2, atk + atk / 2);
        if(res <= 1)
        {
            return 1;
        }
        else
        {
            return res;
        }
    }

    protected virtual void Awake()
    {
        stageData = Utils.JsonToDictionary<string, StageData>("StageData");
        rigid = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponent>();
        itemDropComponent= GetComponent<ItemDropComponent>();

        Init();
    }
    protected virtual void Start()
    {
        healthComponent.DeathEvent += Death;
    }

    protected virtual void FixedUpdate()
    {
        switch(enemyStatus)
        {
            case EnemyStatus.Default:
                TrackingPlayer();
                break;

            case EnemyStatus.KnockBack:
                dirVec = (targetRigid.position - rigid.position).normalized;
                rigid.AddForce(-dirVec * knockBackForce, ForceMode2D.Impulse);             
                break;

            default:
                break;
        }
    }
    protected virtual void OnEnable()
    {
        targetRigid = Player.player.GetComponent<Rigidbody2D>();
        healthComponent.InitHealth(data.hp);
        enemyStatus = EnemyStatus.Default;
    }

    private void OnDisable()
    {
        StopCoroutine(Reset());
    }

    private void Init()
    {
        data = stageData[SceneManager.GetActiveScene().name];

        healthComponent.InitHealth(data.hp);
        exp = data.exp;
        atk = data.atk;
        money = data.money;
    }

    void TrackingPlayer()
    {
        dirVec = targetRigid.position - rigid.position;
        nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        if (dirVec.x >= 0.0f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }

    public void Attacked(int damage)
    {
        healthComponent.HitDamage(damage);
        attackedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void KnockBack()
    {
        enemyStatus = EnemyStatus.KnockBack;
        if(transform.gameObject.activeSelf)
        {
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.15f);
        enemyStatus = EnemyStatus.Default;
    }

    private void Death(object sender, EventArgs eventArgs)
    {
        itemDropComponent.DropMoney(transform.position, money);
        itemDropComponent.DropExp(exp);
        transform.gameObject.SetActive(false);
    }
}

