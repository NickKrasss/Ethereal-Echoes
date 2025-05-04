using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SmoothMoveScr))]
public class FollowObjectScr : MonoBehaviour
{

    [Tooltip("��� ������������� �������")]
    [SerializeField]
    private string followedObjectTag;

    public GameObject followedObject;

    [Tooltip("�������� �� x")]
    [SerializeField]
    private float offset_x = 0f;

    [Tooltip("�������� �� y")]
    [SerializeField]
    private float offset_y = 0f;

    [Tooltip("�������� ����������")]
    [SerializeField]
    private float moveSpeed = 1.0f;

    [Tooltip("����������� �� ���������")]
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

    // ��������� targetMoveVector � ��������� ��� � SmoothMoveScr
    private void UpdateTargetVector()
    {
        smoothScr.targetMoveVector = (followedObject.transform.position - transform.position + new Vector3(offset_x, offset_y, 0));
        if (!dependsOnDistance)
            smoothScr.targetMoveVector = smoothScr.targetMoveVector.normalized;
        smoothScr.targetMoveVector *= moveSpeed;
    }

    private void Start()
    {
        smoothScr = GetComponent<SmoothMoveScr>(); // ������� ��������� SmoothMoveScr
    }

    private void Update()
    {
        if (!followedObject)
        {
            followedObject = GameObject.FindWithTag(followedObjectTag);
            return;
        }
        UpdateTargetVector();
    }
}
