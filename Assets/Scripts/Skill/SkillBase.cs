using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected Collider2D collider;
    protected int skillCode = 123456;
    protected int playerDamage;

    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    protected virtual void OnEnable()
    {
      
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected IEnumerator Delay()
    {
        yield return new WaitForSeconds(SkillManager.inst.GetSkillData(skillCode).delay);
        collider.enabled = true;
    }

    protected virtual IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(SkillManager.inst.GetSkillData(skillCode).destroyTime);
        transform.gameObject.SetActive(false);
    }


    public virtual void UseSkill(Vector2 target, int playerDamage)
    {       
        transform.position = target;
        this.playerDamage = playerDamage;
        collider.enabled = false;

        StartCoroutine(Delay());
        StartCoroutine(DestroySkill());
    }
}
