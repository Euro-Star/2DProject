using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropComponent : MonoBehaviour
{
    public void DropMoney(Vector2 position)
    {
        PoolManager.inst.PoolMoney().transform.position = position;
    }

    public void DropExp(int exp)
    {
        Player.player.abilityComponent.AddExp(exp);
    }
}
    
