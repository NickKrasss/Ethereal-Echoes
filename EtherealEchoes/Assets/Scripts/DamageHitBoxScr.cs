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
    private int damageCount = -1;

    [Tooltip("true - ����� �������� ������� ���� ������ ���. false - ������ ��� ������������")]
    [SerializeField]
    private bool damageEveryTick = false;

    // ������� ����
    private void Hit(HealthScr otherHP)
    {
        if (otherHP.CanHitBy(damageTag))
        {
            otherHP.TakeDamage(damage);
            damageCount--;
            if (damageCount == 0) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageEveryTick) return;
        HealthScr otherHP;
        if (collision.gameObject.TryGetComponent(out otherHP))
        {
            Hit(otherHP);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!damageEveryTick) return;
        HealthScr otherHP;
        if (collision.gameObject.TryGetComponent(out otherHP))
        {
            Hit(otherHP);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageEveryTick) return;
        HealthScr otherHP;
        if (collision.gameObject.TryGetComponent(out otherHP))
        {
            Hit(otherHP);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!damageEveryTick) return;
        HealthScr otherHP;
        if (collision.gameObject.TryGetComponent(out otherHP))
        {
            Hit(otherHP);
        }
    }
}
