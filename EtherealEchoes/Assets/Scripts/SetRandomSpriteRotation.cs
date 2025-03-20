using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSpriteRotation : MonoBehaviour
{
    [SerializeField]
    private float angle = 180f;
    void Start()
    {
        transform.Rotate(new Vector3(0, 0, Random.Range(-angle/2, angle/2)));
    }
}
