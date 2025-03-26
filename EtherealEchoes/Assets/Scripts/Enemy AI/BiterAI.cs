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
public class BiterAI : MonoBehaviour
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

    [Tooltip("Минимальная скорость следования")]
    [SerializeField]
    private float minSpeed = 1.0f;

    [Tooltip("Дальность видимости")]
    [SerializeField]
    private float spotRange = 30f;

    [Tooltip("Ускорение")]
    [SerializeField]
    private float acceleration = 0.1f;

    private bool spottedTarget = false;

    private float curSpeed = 0f;

    private SmoothMoveScr smoothScr;

    [SerializeField]
    private GameObject dmgHitbox;

    private Rigidbody2D rb;

    private Stats stats;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();



        animator.SetFloat("AnimationSpeed", 0f);
        animator.SetFloat("AttackSpeed", stats.AttackSpeed);

        dmgHitbox.transform.localScale = new Vector2(stats.AttackRange, stats.AttackRange);
        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        dmgHitbox.SetActive(false);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<DamageTakable>().damageTakenEvent.AddListener(SpotPlayer);

        stats.level = ((G.Instance.currentLevel - 1) * 10) + Random.Range(1, 4); 
    }

    private void UpdateAnimations()
    {
        if (curSpeed != 0)
        {
            if (transform.position.x > target.transform.position.x)
                sprRenderer.flipX = true;
            else
                sprRenderer.flipX = false;
        }
        animator.SetFloat("MoveRage", (curSpeed - minSpeed) / (stats.MoveSpeed - minSpeed));
    }

    private void UpdateSpeed()
    {
        if (curSpeed <= 0f)
            curSpeed = minSpeed;
        curSpeed = Mathf.Lerp(curSpeed, stats.MoveSpeed, Time.deltaTime * acceleration);
        agent.speed = curSpeed;
    }

    private void CheckAttack()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < stats.AttackRange - (stats.AttackRange / 10) && !animator.GetBool("isAttackingMini"))
        {
            StartCoroutine(Bite());
        }
    }

    private IEnumerator Bite()
    {
        animator.SetBool("isAttackingMini", true);
        yield return new WaitForSeconds(0.75f * (1 / stats.AttackSpeed));
        dmgHitbox.GetComponent<DamageHitBoxScr>().damageCount = 1;
        dmgHitbox.SetActive(true);
        yield return new WaitForSeconds(0.25f * (1 / stats.AttackSpeed));
        dmgHitbox.SetActive(false);
        animator.SetBool("isAttackingMini", false);
    }

    public void SpotPlayer()
    {
        spottedTarget = true;
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
            if (Vector2.Distance(transform.position, target.transform.position) < spotRange)
                SpotPlayer();
        }
        else
        {
            UpdateSpeed();
            UpdateAnimations();
            CheckAttack();
            agent.SetDestination(new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y));
            
            animator.SetFloat("AnimationSpeed", 1f);
        }
    }
}
