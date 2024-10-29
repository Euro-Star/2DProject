using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

public class Money : MonoBehaviour
{
    private int money = 0;
    private int speed = 7;
    private Vector2 dirVec;
    private Vector2 nextVec;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        dirVec = (Player.player.transform.position - this.transform.position).normalized;
        nextVec = dirVec * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    public void SetMoney(int money)
    {
        this.money = Random.Range((int)(money - money * 0.2f), (int)(money + money * 0.2f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Player)
        {
            Player.player.inventory.AddMoney(money);
            transform.gameObject.SetActive(false);
        }
    }
}
