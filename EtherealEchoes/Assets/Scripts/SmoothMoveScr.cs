using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SmoothMoveScr : MonoBehaviour
{
    // Компонент, отвечающий за физику
    private Rigidbody2D rb;

    [Tooltip("Сила \"Трения\". Чем меньше - тем плавнее")]
    [SerializeField]
    private float lerpSpeed = 1.0f;

    // Направление движения обьекта в данный момент
    private Vector2 currentMoveVector;

    [Tooltip("Направление, к которому стремится currentMoveVector. Его необходимо изменять извне")]
    public Vector2 targetMoveVector;

    // Приближает currentMoveVector к targetMoveVector
    private void UpdateCurrentVector()
    {
        currentMoveVector = Vector2.Lerp(currentMoveVector, targetMoveVector, lerpSpeed * Time.deltaTime);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Находит компонент Rigidbody2D
    }

    private void Update()
    {
        UpdateCurrentVector();
        rb.velocity = currentMoveVector; // Двигает обьект по направлению currentMoveVector
    }
}
