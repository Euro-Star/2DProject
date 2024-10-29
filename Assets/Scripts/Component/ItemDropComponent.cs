using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropComponent : MonoBehaviour
{
    public void DropMoney(Vector2 position, int money)
    {
        GameObject gameObject = PoolManager.inst.PoolMoney();
        gameObject.transform.position = position;
        gameObject.GetComponent<Money>().SetMoney(money);
    }

    public void DropExp(int exp)
    {
        Player.player.abilityComponent.AddExp(exp);
    }
}
    
