using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameUtils;

public class LockOn : MonoBehaviour
{
    public static LockOn inst;

    private List<GameObject> overlapEnemy;
    private Rigidbody2D playerRigid;
    private float targetOffset = 1.2f;

    private void Awake()
    {
        inst = this;
        overlapEnemy = new List<GameObject>();
    }
    private void Start()
    {
        playerRigid = Player.player.GetComponent<Rigidbody2D>();   
    }
    private void Update()
    {
        if(overlapEnemy.Count > 0)
        {
            float minDistance = 999f;
            Vector2 minDir = Vector2.zero;
            Vector2 minPos = Vector2.zero;

            foreach (GameObject enemy in overlapEnemy)
            {
                Vector2 dirVec = playerRigid.transform.position - enemy.transform.position;

                if (minDistance > dirVec.magnitude)
                {
                    minDistance = dirVec.magnitude;
                    minDir = dirVec;
                    minPos = enemy.transform.position;
                }
            }

            Player.player.targetVec = -minDir.normalized;
            Player.player.targetPos = new Vector2(minPos.x, minPos.y + targetOffset);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy)
        {
            overlapEnemy.Add(collision.gameObject);
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy)
        {
            overlapEnemy.Remove(collision.gameObject);
        }
    }

    public bool IsOverlapEnemy()
    {
        if(overlapEnemy.Count > 0) 
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }
}
