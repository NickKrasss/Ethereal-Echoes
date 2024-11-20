using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtRange : MonoBehaviour
{
    public float range;

    private float currentRange = 0;
    private Vector2 previousPos;

    private void Awake()
    {
        previousPos = transform.position;
    }
    private void Update()
    {
        currentRange += Vector2.Distance(transform.position, previousPos);
        previousPos = transform.position;
        if (currentRange >= range)
        {
            Destroy(gameObject);
        }
    }
}
