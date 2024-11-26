using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetRandomSpriteFlip : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().flipX = Random.Range(0, 2) == 0;
    }
}
