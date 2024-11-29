using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetRandomSpriteFlip : MonoBehaviour
{
    [SerializeField]
    private bool flipX = false;
    [SerializeField]
    private bool flipY = false;
    private void Start()
    {
        if (flipX)
            GetComponent<SpriteRenderer>().flipX = Random.Range(0, 2) == 0;
        if (flipY)
            GetComponent<SpriteRenderer>().flipY = Random.Range(0, 2) == 0;
    }
}
