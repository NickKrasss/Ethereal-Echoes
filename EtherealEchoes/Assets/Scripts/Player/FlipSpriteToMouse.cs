using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlipSpriteToMouse : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Time.timeScale != 0f)
        {
            spriteRenderer.flipX = WorldMousePosition.GetWorldMousePosition(cam).x < transform.position.x;
        }
    }
}
