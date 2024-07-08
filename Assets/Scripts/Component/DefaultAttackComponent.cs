using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

public class DefaultAttackComponent : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rigid;
    private AbilityComponent abilityComponent;

    public Vector2 targetVec { get; set; }
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        // 가만히 있는 공격 방지
        targetVec = Vector2.up;
    }

    private void Start()
    {
        abilityComponent = Player.player.abilityComponent;
    }

    private void FixedUpdate()
    {
        Vector2 normalVec = targetVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + normalVec);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            
            enemy.Attacked(abilityComponent.GetAtk());
            enemy.KnockBack();
                        
        }
        else if(Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Wall)
        {
            transform.gameObject.SetActive(false);
        }
    }
}
