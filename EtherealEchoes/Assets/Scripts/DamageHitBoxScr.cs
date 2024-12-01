using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // ������� ����
    private void Hit(HealthScr otherHP)
    {
        if (otherHP.CanHitBy(damageTag) && damageCount != 0)
        {
            otherHP.TakeDamage(damage);
            damageCount--;
            if (damageCount == 0 && destroyOnZeroHits) Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (string s in ignoreCollisionTags)
        {
            if (collision.gameObject.CompareTag(s))
                return;
        }
        HealthScr otherHP;
        if (collision.gameObject.TryGetComponent(out otherHP))
        {
            Hit(otherHP);
        }
        else
        {
            if (destroyOnCollision) Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (string s in ignoreCollisionTags)
        {
            if (collision.gameObject.CompareTag(s))
                return;
        }
        HealthScr otherHP;
        if (collision.gameObject.TryGetComponent(out otherHP))
        {
            Hit(otherHP);
        }
        else
        {
            if (destroyOnCollision) Destroy(gameObject);
        }
    }
}
