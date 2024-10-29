using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

public class SlimeBossSkill_3 : MonoBehaviour
{
    [SerializeField] GameObject hitEffect_Prefab;
    [SerializeField] ParticleSystem particle;

    private int damage = 20;
    private int speed = 25;
    private Vector2 nextVec;
    private Rigidbody2D rigid;
    private ParticleSystem.MainModule main;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        main = particle.main;
    }

    private void FixedUpdate()
    {
        nextVec = transform.up * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if (Utils.StringToEnum<GameTag>(collision.tag) == GameTag.PlayerHitBox)
        {
            HitBoxComponent hitBox = collision.GetComponent<HitBoxComponent>();
            hitBox.GetHealthComponent().HitDamage(damage);

            Instantiate(hitEffect_Prefab, transform.position, Quaternion.identity);
            SoundManager.inst.PlaySound(SoundType.Enemy, (int)EnemySound.SlimeBoss_HitSkill);
            Destroy(this.gameObject);
        }
        else if(Utils.StringToEnum<GameTag>(collision.tag) == GameTag.Wall)
        {
            Instantiate(hitEffect_Prefab, transform.position, Quaternion.identity);
            SoundManager.inst.PlaySound(SoundType.Enemy, (int)EnemySound.SlimeBoss_HitSkill);
            Destroy(this.gameObject);
        }
    }

    public void SetParticleRotation(float angle)
    {
        main.startRotation = -Mathf.Deg2Rad * angle;
    }
}
