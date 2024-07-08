using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected SkillData skillData;
    protected Collider2D collider;
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
        yield return new WaitForSeconds(skillData.delay);
        collider.enabled = true;
    }

    protected IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(skillData.destroyTime);
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
