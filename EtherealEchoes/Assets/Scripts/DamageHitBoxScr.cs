using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageHitBoxScr : MonoBehaviour
{
    [Tooltip("Количество урона")]
    public float damage = 0f;

    [Tooltip("Источник урона")]
    [SerializeField]
    private string damageTag = "undefined";

    [Tooltip("Количество нанесений урона. При 0 обьект уничтожается. -1 для бесконечного количества")]
    [SerializeField]
    public int damageCount = -1;

    [SerializeField]
    public float knockbackForce = 0f;

    [Tooltip("true - Будет пытаться нанести урон каждый тик. false - только при столкновении")]
    [SerializeField]
    private bool damageEveryTick = false;

    [Tooltip("Уничтожить при столкновении не с существом")]
    [SerializeField]
    private bool destroyOnCollision = false;

    [Tooltip("Тэги обьектов для игнорирования")]
    [SerializeField]
    private string[] ignoreCollisionTags;

    [SerializeField]
    private bool destroyOnZeroHits = true;

    [SerializeField]
    private bool makeParticlesOnHit = true;

    // Нанести урон
    private void Hit(DamageTakable otherHP, Vector2 collisionPos)
    {
        if (otherHP.CanHitBy(damageTag) && damageCount != 0)
        {
            SpriteSplitParticlesScr otherSplitParticlesScr;
            otherHP.TakeDamage(damage);

            if (makeParticlesOnHit && otherHP.gameObject.TryGetComponent(out otherSplitParticlesScr))
                otherSplitParticlesScr.CreateParticles(otherHP.transform.position);

            Rigidbody2D otherrb;
            if (knockbackForce > 0f && otherHP.gameObject.TryGetComponent(out otherrb))
            {
                Vector2 direction = (collisionPos - (Vector2)transform.position).normalized;
                otherrb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            }

            damageCount--;
            if (damageCount == 0 && destroyOnZeroHits) Destroy(gameObject);
        }
    }

    private void CheckCollision(GameObject obj, Vector2 collisionPos)
    {
        
        foreach (string s in ignoreCollisionTags)
        {
            if (obj.CompareTag(s))
                return;
        }
        DamageTakable otherHP;
        if (obj.TryGetComponent(out otherHP))
        {
            Hit(otherHP, collisionPos);
        }
        else
        {
            if (destroyOnCollision) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject, collision.GetContact(0).point);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject, collision.GetContact(0).point);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject, collision.offset + (Vector2)collision.transform.position);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject, collision.offset + (Vector2)collision.transform.position);
    }
}
