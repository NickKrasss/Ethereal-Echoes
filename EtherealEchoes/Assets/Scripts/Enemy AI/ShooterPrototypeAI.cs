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

    [Tooltip("Время перезарядки")]
    [SerializeField]
    private float reloadTime = 1.0f;

    [Tooltip("Разброс")]
    [SerializeField]
    private float spreadRange = 1.0f;

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

    private float currentReload;

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
        currentReload = reloadTime;
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

        if (distanceToTarget < shootRange)
        {
            if (currentReload <= 0)
            {
                currentReload = reloadTime;
                
            }
        }
    }

    private void Update()
    {
        if (!followedObject)
        {
            followedObject = GameObject.FindWithTag(followedObjectTag);
            return;
        }
        AI();
    }
}
