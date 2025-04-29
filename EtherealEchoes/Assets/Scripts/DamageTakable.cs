using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

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
    private bool animateOnHit = true;
    private float hitProgress = 0f;
    [SerializeField]
    private float hitAnimationSpeed = 3f;

    [SerializeField]
    private Bar bar;

    public UnityEvent damageTakenEvent = new UnityEvent();

    [SerializeField]
    private bool playSoundOnHit;
    [SerializeField]
    private float volumeOnHit;
    [SerializeField]
    private AudioClip[] damageTakenSounds;

    [SerializeField]
    private bool playSoundOnDeath;
    [SerializeField]
    private float volumeOnDeath;
    [SerializeField]
    private AudioClip[] deathSounds;

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
 
        

        damageTakenEvent.Invoke();
        if (playSoundOnHit)
        {
            if (AudioManager.Instance)
                AudioManager.Instance.PlayAudio(damageTakenSounds[UnityEngine.Random.Range(0, damageTakenSounds.Length)], SoundType.SFX, volumeOnHit);
        }
        if (animateOnHit)
        {
            hitProgress = 1f;
        }

        if (gameObject.CompareTag("Player"))
        {
            if (G.Instance.powerUpCards.Contains("Невосприимчивость"))
            {
                int probability = UnityEngine.Random.Range(0, 100);
                if (probability <= G.Instance.blockDamageChance)
                {
                    damage = 0;
                }
            }
        }


        stats.CurrentHealth -= damage;

        if (stats.CurrentHealth < 0) stats.CurrentHealth = 0;

        if (bar)
        {
            bar.Shake();
        }

        if (destroyOnZeroHealth && stats.CurrentHealth <= 0)
        {
            if (playSoundOnDeath)
            {
                if (AudioManager.Instance)
                    AudioManager.Instance.PlayAudio(deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)], SoundType.SFX, volumeOnDeath);
            }
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
        if (hitProgress > 0) hitProgress -= Time.deltaTime * hitAnimationSpeed;
        if (hitProgress < 0) hitProgress = 0;
        if (animateOnHit)
        {
            GetComponent<SpriteRenderer>().material.SetFloat("_HitProgress", hitProgress);
        }
        if (!bar)
        {
            if (gameObject.CompareTag("Player"))
                bar = GameObject.FindGameObjectWithTag("HPBar").GetComponent<Bar>();
        }
        else
        {
            bar.SetValue(stats.CurrentHealth / stats.MaxHealth);
            bar.SetMaxHP(stats.MaxHealth);
        }

    }


}
