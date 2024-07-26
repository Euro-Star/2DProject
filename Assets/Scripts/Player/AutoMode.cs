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
    private HealthComponent healthComponent;
    private Rigidbody2D rigid;
    private float speed;
    private Vector2 targetVec;
    private int skillLen;

    public bool bAutoMode; public bool IsAutoMode() { return bAutoMode; }


    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        skillComponent = Player.player.skillComponent;
        healthComponent = Player.player.healthComponent;

        speed = Player.player.GetSpeed();
        skillLen = SkillManager.inst.SkillLength();

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
                        string skillType = SkillManager.inst.GetSkillData(skillComponent.GetUseSkillIndex(i)).skillType;

                        if (skillType == "Attack" || skillType == "Buff")
                        {
                            skillComponent.Skill(i);
                        }
                        else if(skillType == "Heal")
                        {
                            if(healthComponent.GetHp() < healthComponent.GetMaxHp())
                            {
                                skillComponent.Skill(i);
                            }
                        }
                        
                    }
                }
            }
            else
            {
                if(!(Player.player.inputVec.magnitude > 0f))
                {
                    if (PoolManager.inst.IsActiveEnemy())
                    {
                        float minDistance = 999f;
                        Vector2 minDir = Vector2.zero;
                        Vector2 minPos = Vector2.zero;

                        foreach (GameObject enemy in PoolManager.inst.GetEnemys())
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
            Player.player.UpdateRotate(targetVec.x);
        }          
    }
}
