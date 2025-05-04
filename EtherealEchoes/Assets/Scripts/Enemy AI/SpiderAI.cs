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
public class SpiderAI : MonoBehaviour, EnemyAI
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

    private bool spottedTarget = false;

    private SmoothMoveScr smoothScr;

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



        animator.SetFloat("speed", 0f);

        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GetComponent<DamageTakable>().damageTakenEvent.AddListener(SpotPlayer);

        stats.level = ((G.Instance.currentLevel - 1) * 10) + Random.Range(1, 4);
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
    }


    public void SpotPlayer()
    {
        spottedTarget = true;
        dmgHitbox.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
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
            agent.SetDestination(new Vector2(target.transform.position.x - offset_x, target.transform.position.y - offset_y));
            animator.SetFloat("speed", 1f);
            if (dmgHitbox == null)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Spot()
    {
        spottedTarget = true;
    }
}
