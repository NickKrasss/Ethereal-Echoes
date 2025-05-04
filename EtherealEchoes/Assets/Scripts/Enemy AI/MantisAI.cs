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
public class MantisAI : MonoBehaviour
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

    [SerializeField]
    private GameObject dmgHitbox;

    private Rigidbody2D rb;

    private Stats stats;

    private Vector2 posOffset;

    private float walkTime = 0f;

    private float meleeCooldown = 0f;

    private float rangedCooldown = 4f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float rangeInaccuracy;

    [SerializeField]
    private AudioClip[] shootAudioClips;

    [SerializeField]
    private float shootVolume;

    [SerializeField]
    private int shotCount;

    private float worldTime;

    void Start()
    {
        worldTime = G.Instance.currentWorldObj.GetComponent<WorldObject>().worldTime;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();

        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        dmgHitbox.SetActive(false);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<DamageTakable>().damageTakenEvent.AddListener(() => { spottedTarget = true; });

        stats.level = ((G.Instance.currentLevel - 1) * 10) + Random.Range(1, 4);

    }

    private void UpdateFlip()
    {
        if (agent.speed != 0)
        {
            if (transform.position.x > target.transform.position.x)
                sprRenderer.flipX = true;
            else
                sprRenderer.flipX = false;
        }
    }



    public void MeleeAttackEvent()
    {
        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        StartCoroutine(MeleeCoroutine());
    }

    private IEnumerator MeleeCoroutine()
    {
        dmgHitbox.GetComponent<DamageHitBoxScr>().damageCount = 1;
        dmgHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        dmgHitbox.SetActive(false);
    }

    public void RangeAttackEvent()
    {
        for (int i = 0; i < shotCount; i++)
        {
            Shoot(new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y));
            rangedCooldown = Random.Range(0.7f, 1.4f) / stats.AttackSpeed;
        }
    }

    private void Shoot(Vector2 targetPos)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.SetParent(G.Instance.currentWorldObj.transform);
        bullet.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();
        scr.targetMoveVector = (targetPos - (Vector2)transform.position).normalized * stats.BulletSpeed * Random.Range(0.6f, 1.2f);
        float spread = UnityEngine.Random.Range(-stats.SpreadDegrees / 2, stats.SpreadDegrees / 2) * Mathf.Deg2Rad;
        float x = scr.targetMoveVector.x;
        float y = scr.targetMoveVector.y;
        scr.targetMoveVector = new Vector2(x * Mathf.Cos(spread) - y * Mathf.Sin(spread), x * Mathf.Sin(spread) + y * Mathf.Cos(spread));
        
        bullet.transform.localScale = new Vector3(Random.Range(0.3f, 0.8f), Random.Range(0.3f, 0.8f), 1);

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
        if (Vector2.Distance(targetPos, transform.position) < 2 && meleeCooldown <= 0f)
        {
            animator.SetBool("isMeleeAttacking", true);
            meleeCooldown = 2f;
        }
        else
            animator.SetBool("isMeleeAttacking", false);

        if (Vector2.Distance(targetPos, transform.position) >= 1 && Vector2.Distance(targetPos, transform.position) <= 6 && rangedCooldown <= 0f)
        {
            animator.SetBool("isRangeAttacking", true);
        }
        else
            animator.SetBool("isRangeAttacking", false);

        if (meleeCooldown > 0f)
            meleeCooldown -= Time.deltaTime;
        if (rangedCooldown > 0f)
            rangedCooldown -= Time.deltaTime;
    }

    private void Walk()
    {
        
        if (!agent.hasPath || walkTime <= 0)
        {
            float ranAngle = Random.Range(0, Mathf.PI*2);
            posOffset = new Vector2(Mathf.Sin(ranAngle), Mathf.Cos(ranAngle));
            posOffset *= radius * Random.Range(0.5f, 1f);
            walkTime = 2f;
        }
        walkTime -= Time.deltaTime;
        Vector2 targetPos = new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y);

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("AttackLeft"))
        {
            agent.SetDestination(targetPos);
        }
        else
        {
            agent.SetDestination(targetPos + posOffset);
        }
        agent.speed = stats.MoveSpeed;
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
            }
        }
        else
        {
            animator.SetBool("spottedPlayer", true);
            UpdateFlip();
            Walk();
            CheckAttack();
        }
    }
}
