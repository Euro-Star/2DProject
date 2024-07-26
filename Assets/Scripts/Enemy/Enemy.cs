using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D targetRigid;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float knockBackForce;

    private Rigidbody2D rigid;
    private HealthComponent healthComponent;
    private ItemDropComponent itemDropComponent;

    private int exp = 50;
    private int atk = 1;

    private Vector2 dirVec;
    private Vector2 nextVec;
    private EnemyStatus enemyStatus = EnemyStatus.Default;

    public int GetAtk() { return atk; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        healthComponent = GetComponent<HealthComponent>();
        itemDropComponent= GetComponent<ItemDropComponent>();
    }
    private void Start()
    {
        healthComponent.DeathEvent += Death;
    }

    private void FixedUpdate()
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
    private void OnEnable()
    {
        targetRigid = Player.player.GetComponent<Rigidbody2D>();
        healthComponent.InitHealth(10);
        enemyStatus = EnemyStatus.Default;
    }

    private void OnDisable()
    {
        StopCoroutine(Reset());
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
        itemDropComponent.DropMoney(transform.position);
        itemDropComponent.DropExp(exp);
        transform.gameObject.SetActive(false);
    }
}

