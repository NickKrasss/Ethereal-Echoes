using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SmoothMoveScr))]
public class ShooterPrototypeAI : MonoBehaviour
{
    [Tooltip("Тэг преследуемого обьекта")]
    [SerializeField]
    private string followedObjectTag;

    public GameObject followedObject;

    [Tooltip("Смещение по x")]
    [SerializeField]
    private float offset_x = 0f;

    [Tooltip("Смещение по y")]
    [SerializeField]
    private float offset_y = 0f;

    [Tooltip("Скорость следования")]
    [SerializeField]
    private float moveSpeed = 1.0f;

    [Tooltip("Расстояние до цели для стрельбы")]
    [SerializeField]
    private float shootRange = 1.0f;

    [Tooltip("Дистанция, до которой обьект будет подходить")]
    [SerializeField]
    private float stayRange = 1.0f;

    [Tooltip("Чем стреляет")]
    [SerializeField]
    private GameObject projectileObj;

    [Tooltip("Отходит ли, если близко к обьекту")]
    [SerializeField]
    private bool retreat = false;

    [Tooltip("Скорость при отходе")]
    [SerializeField]
    private float retreatSpeed = 1.0f;

    private SmoothMoveScr smoothScr;

    [SerializeField]
    private float bulletSpeed = 5f;

    [SerializeField]
    private float bulletDamage = 8;

    [SerializeField]
    private float bulletKnockback = 5f;

    [SerializeField]
    private float fireRate = 4;

    [SerializeField]
    private float spreadDegrees = 0;

    [SerializeField]
    private float range = 5;

    [SerializeField]
    private float rangeInaccuracy = 5;

    private float reload = 0f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
            if (moveSpeed < 0f)
                moveSpeed = 0f;
        }
    }

    private void Start()
    {
        smoothScr = GetComponent<SmoothMoveScr>(); // Находит компонент SmoothMoveScr
    }

    private void Update()
    {
        if (!followedObject)
        {
            followedObject = GameObject.FindWithTag(followedObjectTag);
            return;
        }

        if (reload <= 0f && Vector2.Distance(transform.position, followedObject.transform.position) <= shootRange)
        {
            Shoot();
        }
        if (reload > 0f)
            reload -= Time.deltaTime;
        else if (reload < 0f) reload = 0f;
        AI();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(projectileObj, transform.position, Quaternion.identity);
        bullet.GetComponent<DamageHitBoxScr>().damage = bulletDamage;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();
        scr.targetMoveVector = (followedObject.transform.position - transform.position).normalized * bulletSpeed;
        float spread = UnityEngine.Random.Range(-spreadDegrees / 2, spreadDegrees / 2) * Mathf.Deg2Rad;
        float x = scr.targetMoveVector.x;
        float y = scr.targetMoveVector.y;
        scr.targetMoveVector = new Vector2(x * Mathf.Cos(spread) - y * Mathf.Sin(spread), x * Mathf.Sin(spread) + y * Mathf.Cos(spread));
        reload = 1 / fireRate;

        DestroyAtRange rangeScr = bullet.GetComponent<DestroyAtRange>();
        if (rangeScr)
        {
            rangeScr.range = range + UnityEngine.Random.Range(-rangeInaccuracy / 2, rangeInaccuracy / 2);
        }
    }

    private void AI()
    {
        float distanceToTarget = Vector2.Distance(transform.position, followedObject.transform.position);


        if (distanceToTarget > stayRange)
        {
            smoothScr.targetMoveVector = (followedObject.transform.position - transform.position + new Vector3(offset_x, offset_y, 0));
            smoothScr.targetMoveVector = smoothScr.targetMoveVector.normalized;
            smoothScr.targetMoveVector *= moveSpeed;
        }
        else
        {
            smoothScr.targetMoveVector = (followedObject.transform.position - transform.position + new Vector3(offset_x, offset_y, 0));
            smoothScr.targetMoveVector = smoothScr.targetMoveVector.normalized;
            smoothScr.targetMoveVector *= -retreatSpeed;
        }
    }
}
