using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameUtils;
using UnityEngine.SceneManagement;

public class LockOn : MonoBehaviour
{
    private List<GameObject> overlapEnemy;
    private List<GameObject> debug_overlapEnemy;
    private Rigidbody2D playerRigid;
    private float targetOffset = 1.2f;
    private CircleCollider2D coll;

    private void Awake()
    {
        overlapEnemy = new List<GameObject>();
        debug_overlapEnemy = new List<GameObject>();
        coll = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        playerRigid = Player.player.GetComponent<Rigidbody2D>();

        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Stage_5")
        {
            coll.radius = 2f;
        }
        else
        {
            coll.radius = 0.5f;
        }

        overlapEnemy.Clear();
    }

    private void FixedUpdate()
    {
        if (overlapEnemy.Count > 0)
        {
            float minDistance = 999f;
            Vector2 minDir = Vector2.zero;
            Vector2 minPos = Vector2.zero;

            foreach (GameObject enemy in overlapEnemy)
            {
                if (!enemy.activeSelf)
                {
                    debug_overlapEnemy.Add(enemy);
                }

                if (enemy != null)
                {
                    Vector2 dirVec = playerRigid.transform.position - enemy.transform.position;
        
                    if (minDistance > dirVec.magnitude)
                    {
                        minDistance = dirVec.magnitude;
                        minDir = dirVec;
                        minPos = enemy.transform.position;
                    }
                }
            }
        
            Player.player.targetVec = -minDir.normalized;
            Player.player.targetPos = new Vector2(minPos.x, minPos.y + targetOffset);

            if(debug_overlapEnemy.Count > 0)
            {
                DebugOverlapEnemy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy || Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Boss)
        {
            overlapEnemy.Add(collision.gameObject);
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Enemy || Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Boss)
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

    private void DebugOverlapEnemy()
    {
        foreach (GameObject enemy in debug_overlapEnemy)
        {
            overlapEnemy.Remove(enemy);
        }

        debug_overlapEnemy.Clear();
        Debug.Log("debugging");
    }
}
