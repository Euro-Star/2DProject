using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropComponent : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = Player.player.GetComponent<Player>();
    }

    public void DropMoney(Vector2 position)
    {
        PoolManager.inst.PoolMoney().transform.position = position;
    }

    public void DropExp(int exp)
    {
        player.abilityComponent.AddExp(exp);
    }
}
    
