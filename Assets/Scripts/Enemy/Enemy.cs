using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameUtils;
using UnityEngine.SceneManagement;


public class Enemy : EnemyBase
{
    [SerializeField] protected float speed = 2;
    [SerializeField] protected float knockBackForce = 1.5f;

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

    public override void Attacked(int damage)
    {
        base.Attacked(damage);        
        KnockBack();      
    }

    private void TrackingPlayer()
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

    public void KnockBack()
    {
        enemyStatus = EnemyStatus.KnockBack;
        if(transform.gameObject.activeSelf)
        {
            StartCoroutine(Reset());
        }
    }
}

