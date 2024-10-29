using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
public class AutoMode : MonoBehaviour
{
    [SerializeField]
    LockOn lockOn;

    private Rigidbody2D rigid;
    private float speed;
    private Vector2 targetVec;
    private int skillLen;

    public bool bAutoMode;
    public bool IsAutoMode() { return bAutoMode; }
    public void SetAutoMode(bool bAuto) { bAutoMode = bAuto; }

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = Player.player.GetSpeed();
        skillLen = SkillManager.inst.SkillLength();

        bAutoMode = true;
    }

    private void FixedUpdate()
    {
        if (Player.player.IsDeath())
        {
            return;
        }

        if (bAutoMode)
        {
            if(lockOn.IsOverlapEnemy())
            {
                targetVec = Vector2.zero;
                bool[] bArray = Player.player.skillComponent.IsSkillsAvailable();

                for (int i = 0; i < skillLen; ++i)
                {
                    if (!bArray[i])
                    {
                        string skillType = SkillManager.inst.GetSkillData(Player.player.skillComponent.GetUseSkillIndex(i)).skillType;

                        if (skillType == "Attack" || skillType == "Buff")
                        {
                            Player.player.skillComponent.Skill(i);
                        }
                        else if(skillType == "Heal")
                        {
                            if(Player.player.healthComponent.GetHp() < Player.player.healthComponent.GetMaxHp())
                            {
                                Player.player.skillComponent.Skill(i);
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
