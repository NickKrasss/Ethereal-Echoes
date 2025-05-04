using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(DamageTakable))]
public class RabbitAI : MonoBehaviour
{
    private SpriteRenderer sprRenderer;

    private GameObject target;

    private Animator animator;

    private NavMeshAgent agent;

    [Tooltip("Смещение по x")]
    [SerializeField]
    private float offset_x = 0f;

    [Tooltip("Смещение по y")]
    [SerializeField]
    private float offset_y = 0f;

    [Tooltip("Дальность видимости")]
    [SerializeField]
    private float spotRange = 30f;

    [SerializeField]
    private float radius = 30f;

    private bool spottedTarget = false;

    private Rigidbody2D rb;

    private Stats stats;

    private float rangedCooldown = 1f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float rangeInaccuracy;

    [SerializeField]
    private AudioClip[] shootAudioClips;

    [SerializeField]
    private float shootVolume;

    private float worldTime;
    void Start()
    {
        worldTime = G.Instance.currentWorldObj.GetComponent<WorldObject>().worldTime;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();

        animator.SetFloat("speedMult", 0f);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<DamageTakable>().damageTakenEvent.AddListener(() => { spottedTarget = true; animator.SetBool("spotted", true); });

        stats.level = ((G.Instance.currentLevel - 1) * 10) + Random.Range(1, 4);

    }

    private void UpdateFlip()
    {
        if (spottedTarget)
        {
            if (transform.position.x > target.transform.position.x)
                sprRenderer.flipX = true;
            else
                sprRenderer.flipX = false;
        }
    }

    public void RangeAttackEvent()
    {
        Shoot(new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y));
        
    }

    private void Shoot(Vector2 targetPos)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.SetParent(G.Instance.currentWorldObj.transform);
        bullet.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();
        scr.targetMoveVector = (targetPos - (Vector2)transform.position).normalized * stats.BulletSpeed;
        float spread = UnityEngine.Random.Range(-stats.SpreadDegrees / 2, stats.SpreadDegrees / 2) * Mathf.Deg2Rad;
        float x = scr.targetMoveVector.x;
        float y = scr.targetMoveVector.y;
        scr.targetMoveVector = new Vector2(x * Mathf.Cos(spread) - y * Mathf.Sin(spread), x * Mathf.Sin(spread) + y * Mathf.Cos(spread));
        
        bullet.transform.localScale = new Vector3(Random.Range(0.8f, 0.9f), Random.Range(0.8f, 0.9f), 1);

        DestroyAtRange rangeScr = bullet.GetComponent<DestroyAtRange>();
        if (rangeScr)
        {
            rangeScr.range = stats.AttackRange + UnityEngine.Random.Range(-rangeInaccuracy / 2, rangeInaccuracy / 2);
        }
        if (AudioManager.Instance)
            AudioManager.Instance.PlayAudio(shootAudioClips[UnityEngine.Random.Range(0, shootAudioClips.Length)], SoundType.SFX, shootVolume, 0.01f, 0.05f);
    }

    private void CheckAttack()
    {
        Vector2 targetPos = new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y);
        if (Vector2.Distance(targetPos, transform.position) <= radius + 1f && rangedCooldown <= 0f)
        {
            animator.SetBool("isAttacking", true);
            rangedCooldown = Random.Range(0.9f, 1.1f) / stats.AttackSpeed;
        }
        else
            animator.SetBool("isAttacking", false);

        if (rangedCooldown > 0f)
            rangedCooldown -= Time.deltaTime;
    }

    private void Walk()
    {
        Vector2 targetPos = new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y);
        agent.SetDestination(targetPos);
        if (Vector2.Distance(targetPos, transform.position) >= radius - 1f) agent.speed = stats.MoveSpeed;
        else agent.speed = 0;
        animator.SetFloat("speedMult", agent.speed / stats.MoveSpeed);
    }

    void Update()
    {
        if (!target)
        {
            target = G.Instance.playerObj;
            return;
        }

        if (!spottedTarget)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < spotRange && worldTime - G.Instance.currentTime > 3)
            {
                spottedTarget = true;
                animator.SetBool("spotted", true);
            }
        }
        else
        {
            UpdateFlip();
            Walk();
            CheckAttack();
        }
    }
}
