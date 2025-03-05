using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stats))]
public class DamageTakable : MonoBehaviour
{
    private Collider2D col;

    private Stats stats;

    [Tooltip("Источники урона")]
    [SerializeField]
    private string[] damageTags;

    [Tooltip("Время неуязвимости после получения урона")]
    [SerializeField]
    private float invincibleTime = 0.1f;

    private float currentInvincibleTime = 0f;

    [Tooltip("Уничтожить обьект при здоровье = 0")]
    [SerializeField]
    private bool destroyOnZeroHealth = true;

    [Tooltip("Обьект неуязвим")]
    [SerializeField]
    private bool invincible = false;

    [SerializeField]
    private Bar bar;

    public bool IsInvincible()
    {
        return invincible || currentInvincibleTime > 0;
    }

    public bool CanHitBy(string dmgTag)
    { 
        return !IsInvincible() && damageTags.Contains(dmgTag);
    }

    // Получить урон.
    public void TakeDamage(float damage)
    {
        if (IsInvincible()) return;
        damage = stats.GetDamageTaken(damage);
        if (damage < 0) return;

        stats.CurrentHealth -= damage;
        if (stats.CurrentHealth < 0) stats.CurrentHealth = 0;

        if (bar)
        {
            bar.Shake();
        }

        if (destroyOnZeroHealth && stats.CurrentHealth <= 0)
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
        stats = GetComponent<Stats>();
    }

    private void Update()
    {
        if (currentInvincibleTime > 0) currentInvincibleTime -= Time.deltaTime;
        if (currentInvincibleTime < 0) currentInvincibleTime = 0;                   // Просчитывает кадры неуязвимости

        if (!bar) 
        {
            if (gameObject.CompareTag("Player"))
                bar = GameObject.FindGameObjectWithTag("HPBar").GetComponent<Bar>(); 
        }
        else bar.SetValue(stats.CurrentHealth / stats.MaxHealth);

    }


}
