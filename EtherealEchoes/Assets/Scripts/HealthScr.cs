using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class HealthScr : MonoBehaviour
{
    private Collider2D col;

    [Tooltip("������� ��������")]
    public float health = 0f;

    [Tooltip("������������ ��������")]
    public float maxHealth = 0f;

    [Tooltip("��������� �����")]
    [SerializeField]
    private string[] damageTags;

    [Tooltip("����� ������������ ����� ��������� �����")]
    [SerializeField]
    private float invincibleTime = 0.1f;

    private float currentInvincibleTime = 0f;

    [Tooltip("���������� ������ ��� �������� = 0")]
    [SerializeField]
    private bool destroyOnZeroHealth = true;

    [Tooltip("������ ��������")]
    [SerializeField]
    private bool invincible = false;

    public bool hittedThatFrame = false;

    public bool IsInvincible()
    {
        return invincible || currentInvincibleTime > 0;
    }

    public bool CanHitBy(string dmgTag)
    { 
        return !IsInvincible() && damageTags.Contains(dmgTag);
    }

    // �������� ����.
    public void TakeDamage(float damage)
    {
        if (IsInvincible()) return;
        if (damage < 0) return;

        hittedThatFrame = true;

        health -= damage;
        if (health < 0) health = 0;

        if (destroyOnZeroHealth && health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            currentInvincibleTime = invincibleTime;
        }
    }

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        hittedThatFrame = false;
        if (currentInvincibleTime > 0) currentInvincibleTime -= Time.deltaTime;
        if (currentInvincibleTime < 0) currentInvincibleTime = 0;                   // ������������ ����� ������������
    }


}
