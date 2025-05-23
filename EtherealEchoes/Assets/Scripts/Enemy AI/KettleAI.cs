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
public class KettleAI : MonoBehaviour, EnemyAI
{
    private SpriteRenderer sprRenderer;

    private GameObject target;

    private Animator animator;

    private NavMeshAgent agent;

    [Tooltip("�������� �� x")]
    [SerializeField]
    private float offset_x = 0f;

    [Tooltip("�������� �� y")]
    [SerializeField]
    private float offset_y = 0f;

    [Tooltip("��������� ���������")]
    [SerializeField]
    private float spotRange = 30f;


    private bool spottedTarget = false;

    private float curSpeed = 0f;

    [SerializeField]
    private GameObject dmgHitbox;

    private Rigidbody2D rb;

    private Stats stats;

    private float worldTime;

    void Start()
    {
        worldTime = G.Instance.currentWorldObj.GetComponent<WorldObject>().worldTime;

        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();



        animator.SetFloat("moveSpeed", 0f);

        dmgHitbox.transform.localScale = new Vector2(stats.AttackRange, stats.AttackRange);
        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        dmgHitbox.SetActive(false);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<DamageTakable>().damageTakenEvent.AddListener(SpotPlayer);

        stats.level = ((G.Instance.currentLevel - 1) * 10) + Random.Range(1, 4);
    }

    public void Spot()
    {
        spottedTarget = true;
    }
    private void UpdateAnimations()
    {
        if (agent.speed != 0)
        {
            if (transform.position.x > target.transform.position.x)
                sprRenderer.flipX = true;
            else
                sprRenderer.flipX = false;
        }
    }

    private void UpdateSpeed()
    {
        agent.speed = stats.MoveSpeed;
        animator.SetFloat("moveSpeed", agent.speed);
    }

    private void CheckAttack()
    {
        if (Vector2.Distance(transform.position, target.transform.position) < spotRange*0.75f && !animator.GetBool("isAttacking") && stats.CurrentEnergy >= 50)
        {
            StartCoroutine(Bite());
            stats.CurrentEnergy -= 50;
        }
    }

    public void DashEvent()
    {
        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        dmgHitbox.GetComponent<DamageHitBoxScr>().damageCount = 1;
        dmgHitbox.SetActive(true);
        Vector2 targetPos = (new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y));
        rb.AddForce((targetPos-(Vector2)transform.position).normalized * stats.BulletSpeed, ForceMode2D.Impulse);
    }

    private IEnumerator Bite()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1.2f);
        dmgHitbox.SetActive(false);
        animator.SetBool("isAttacking", false);
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
            if (Vector2.Distance(transform.position, target.transform.position) < spotRange && worldTime - G.Instance.currentTime > 3)
                SpotPlayer();
        }
        else
        {
            UpdateSpeed();
            UpdateAnimations();
            CheckAttack();
            agent.SetDestination(new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y));
        }
    }
}
