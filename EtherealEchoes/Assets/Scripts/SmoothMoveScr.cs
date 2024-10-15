using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SmoothMoveScr : MonoBehaviour
{
    // ���������, ���������� �� ������
    private Rigidbody2D rb;

    [Tooltip("���� \"������\". ��� ������ - ��� �������")]
    [SerializeField]
    private float lerpSpeed = 1.0f;

    // ����������� �������� ������� � ������ ������
    private Vector2 currentMoveVector;

    [Tooltip("�����������, � �������� ��������� currentMoveVector. ��� ���������� �������� �����")]
    public Vector2 targetMoveVector;

    // ���������� currentMoveVector � targetMoveVector
    private void UpdateCurrentVector()
    {
        currentMoveVector = Vector2.Lerp(currentMoveVector, targetMoveVector, lerpSpeed * Time.deltaTime);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // ������� ��������� Rigidbody2D
    }

    private void Update()
    {
        UpdateCurrentVector();
        rb.velocity = currentMoveVector; // ������� ������ �� ����������� currentMoveVector
    }
}
