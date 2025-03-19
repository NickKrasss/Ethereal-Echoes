using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(Collider2D))]
public class DamageHitBoxScr : MonoBehaviour
{
    [Tooltip("���������� �����")]
    public float damage = 0f;

    [Tooltip("�������� �����")]
    [SerializeField]
    private string damageTag = "undefined";

    [Tooltip("���������� ��������� �����. ��� 0 ������ ������������. -1 ��� ������������ ����������")]
    [SerializeField]
    public int damageCount = -1;

    [Tooltip("true - ����� �������� ������� ���� ������ ���. false - ������ ��� ������������")]
    [SerializeField]
    private bool damageEveryTick = false;

    [Tooltip("���������� ��� ������������ �� � ���������")]
    [SerializeField]
    private bool destroyOnCollision = false;

    [Tooltip("���� �������� ��� �������������")]
    [SerializeField]
    private string[] ignoreCollisionTags;

    [SerializeField]
    private bool destroyOnZeroHits = true;

    [SerializeField]
    private bool makeParticlesOnHit = true;

    // ������� ����
    private void Hit(DamageTakable otherHP, Vector2 pos)
    {
        if (otherHP.CanHitBy(damageTag) && damageCount != 0)
        {
            SpriteSplitParticlesScr otherSplitParticlesScr;
            otherHP.TakeDamage(damage);
            if (makeParticlesOnHit && otherHP.gameObject.TryGetComponent(out otherSplitParticlesScr))
                otherSplitParticlesScr.CreateParticles(pos);
            damageCount--;
            if (damageCount == 0 && destroyOnZeroHits) Destroy(gameObject);
        }
    }

    private void CheckCollision(GameObject obj, Vector2 pos)
    {
        
        foreach (string s in ignoreCollisionTags)
        {
            if (obj.CompareTag(s))
                return;
        }
        DamageTakable otherHP;
        if (obj.TryGetComponent(out otherHP))
        {
            Hit(otherHP, pos);
        }
        else
        {
            if (destroyOnCollision) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject, collision.transform.position);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject, collision.transform.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject, collision.transform.position);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject, collision.transform.position);
    }
}
