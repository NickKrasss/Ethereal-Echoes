using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
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

    [Tooltip("Максимальная скорость следования")]
    [SerializeField]
    private float maxSpeed = 1.0f;

    [Tooltip("Дальность видимости")]
    [SerializeField]
    private float spotRange = 30f;

    [Tooltip("Ускорение")]
    [SerializeField]
    private float acceleration = 0.1f;

    [Tooltip("Дальность укуса")]
    [SerializeField]
    private float attackRange = 0.5f;

    [Tooltip("Скорость атаки")]
    [SerializeField]
    private float attackSpeed = 1f;

    private bool spottedTarget = false;

    private float curSpeed = 0f;

    private SmoothMoveScr smoothScr;

    [SerializeField]
    private GameObject dmgHitbox;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetFloat("AnimationSpeed", 0f);
        animator.SetFloat("AttackSpeed", attackSpeed);
        dmgHitbox.transform.localScale = new Vector2(attackRange, attackRange);
        dmgHitbox.SetActive(false);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
        animator.SetFloat("MoveRage", (curSpeed - minSpeed)/(maxSpeed - minSpeed));
    }

    private void UpdateSpeed()
    {
        if (curSpeed <= 0f)
            curSpeed = minSpeed;
        curSpeed = Mathf.Lerp(curSpeed, maxSpeed, Time.deltaTime * acceleration);
        agent.speed = curSpeed;
    }

    private void CheckAttack()
    { 
        if (Vector2.Distance(transform.position, target.transform.position) < attackRange - (attackRange/10) && !animator.GetBool("isAttackingMini"))
        {
            StartCoroutine(Bite());
        }
    }

    private IEnumerator Bite()
    {
        animator.SetBool("isAttackingMini", true);
        yield return new WaitForSeconds(0.75f * attackSpeed);
        dmgHitbox.GetComponent<DamageHitBoxScr>().damageCount = 1;
        dmgHitbox.SetActive(true);
        yield return new WaitForSeconds(0.25f * attackSpeed);
        dmgHitbox.SetActive(false);
        animator.SetBool("isAttackingMini", false);
    }

    void Update()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        if (!spottedTarget)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < spotRange)
                spottedTarget = true;
        }
        else
        {
            UpdateSpeed();
            UpdateAnimations();
            CheckAttack();
            agent.SetDestination(target.transform.position);
            animator.SetFloat("AnimationSpeed", 1f);
        }
    }
}
