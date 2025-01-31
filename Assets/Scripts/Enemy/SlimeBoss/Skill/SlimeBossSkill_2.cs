using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

public class SlimeBossSkill_2 : MonoBehaviour
{
    private int damage = 15;
    private bool bHit = false;

    private void OnEnable()
    {
        bHit = false;
    }

    private void Start()
    {
        StartCoroutine(DestroySkill());
        StartCoroutine(HitVerdict());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!bHit)
        {
            if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.PlayerHitBox)
            {
                HitBoxComponent hitBox = collision.GetComponent<HitBoxComponent>();
                IHealth healthComp = hitBox.GetHealthComponent();
                healthComp.HitDamage(damage);

                bHit = true;
            }
        }      
    }

    private IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }

    private IEnumerator HitVerdict()
    {
        yield return new WaitForSeconds(0.15f);
        bHit = true;
    }
}
