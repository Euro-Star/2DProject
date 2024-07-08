using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

public class Money : MonoBehaviour
{
    int money = 0;

    private void OnEnable()
    {
        SetMoney();
    }

    public void SetMoney()
    {
        money = Random.Range(1, 10);
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
