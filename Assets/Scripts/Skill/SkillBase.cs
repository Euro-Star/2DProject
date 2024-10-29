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
        Player.player.skillComponent.SetSkillDelay(false);
        StartCoroutine(DestroySkill());
    }

    protected virtual IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(SkillManager.inst.GetSkillData(skillCode).destroyTime);
        transform.gameObject.SetActive(false);
    }


    public virtual void UseSkill(Vector2 target, int playerDamage)
    {
        collider.enabled = false;
        Player.player.skillComponent.SetSkillDelay(true);

        if (SkillManager.inst.GetSkillType(skillCode) == "Attack")
        {
            transform.position = target;
        }
        else
        {
            transform.position = Player.player.transform.position;
        }

        this.playerDamage = playerDamage;

        StartCoroutine(Delay());
    }
}
