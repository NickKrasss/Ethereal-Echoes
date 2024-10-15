using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SmoothMoveScr))]
public class FollowObjectScr : MonoBehaviour
{

    [Tooltip("Преследуемый обьект")]
    [SerializeField]
    private GameObject followedObject;

    [Tooltip("Смещение по x")]
    [SerializeField]
    private float offset_x = 0f;

    [Tooltip("Смещение по y")]
    [SerializeField]
    private float offset_y = 0f;

    [Tooltip("Скорость следования")]
    [SerializeField]
    private float moveSpeed = 1.0f;

    [Tooltip("Зависимость от дистанции")]
    [SerializeField]
    private bool dependsOnDistance = true;

    private SmoothMoveScr smoothScr;

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

    // Вычисляет targetMoveVector и обновляет его в SmoothMoveScr
    private void UpdateTargetVector()
    {
        smoothScr.targetMoveVector = (followedObject.transform.position - transform.position + new Vector3(offset_x, offset_y, 0));
        if (!dependsOnDistance)
            smoothScr.targetMoveVector = smoothScr.targetMoveVector.normalized;
        smoothScr.targetMoveVector *= moveSpeed;
    }

    private void Start()
    {
        smoothScr = GetComponent<SmoothMoveScr>(); // Находит компонент SmoothMoveScr
    }

    private void Update()
    {
        UpdateTargetVector();
    }
}
