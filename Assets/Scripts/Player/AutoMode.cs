using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using UnityEditor.EditorTools;

public class AutoMode : MonoBehaviour
{
    [SerializeField]
    LockOn lockOn;

    private SkillComponent skillComponent;
    private PoolManager poolManager;
    private Rigidbody2D rigid;
    private Player player;
    private float speed;
    private Vector2 targetVec;
    private int skillLen;

    public bool bAutoMode; public bool IsAutoMode() { return bAutoMode; }


    private void OnEnable()
    {
        skillComponent = GetComponent<SkillComponent>();
        rigid = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        speed = player.GetSpeed();
        poolManager = PoolManager.inst;
        skillLen = SkillManager.instance.SkillLength();

        bAutoMode = true;
    }

    private void FixedUpdate()
    {
        if(bAutoMode)
        {
            if(lockOn.IsOverlapEnemy())
            {
                targetVec = Vector2.zero;
                bool[] bArray = skillComponent.IsSkillsAvailable();

                for (int i = 0; i < skillLen; ++i)
                {
                    if (!bArray[i])
                    {
                        skillComponent.Skill(i);
                    }
                }
            }
            else
            {
                if(!(player.inputVec.magnitude > 0f))
                {
                    if (poolManager.IsActiveEnemy())
                    {
                        float minDistance = 999f;
                        Vector2 minDir = Vector2.zero;
                        Vector2 minPos = Vector2.zero;

                        foreach (GameObject enemy in poolManager.GetEnemys())
                        {
                            if (enemy.activeSelf == true)
                            {
                                Vector2 dirVec = rigid.transform.position - enemy.transform.position;

                                if (minDistance > dirVec.magnitude)
                                {
                                    minDistance = dirVec.magnitude;
                                    minDir = dirVec;
                                    minPos = enemy.transform.position;
                                }
                            }
                        }

                        targetVec = -minDir.normalized;
                        Vector2 normalVec = targetVec * speed * Time.fixedDeltaTime;
                        rigid.MovePosition(rigid.position + normalVec);
                    }
                }         
            }
        }
    }

    private void LateUpdate()
    {
        if (bAutoMode)
        {
            player.UpdateRotate(targetVec.x);
        }          
    }
}