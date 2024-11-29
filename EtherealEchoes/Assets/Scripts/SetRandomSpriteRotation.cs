using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSpriteRotation : MonoBehaviour
{
    [SerializeField]
    private float angle = 180f;
    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(Random.Range(-angle, angle), Vector3.forward);
    }
}
