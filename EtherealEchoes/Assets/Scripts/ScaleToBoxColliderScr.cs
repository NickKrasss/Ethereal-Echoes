using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToBoxColliderScr : MonoBehaviour
{
    void Awake()
    {
        BoxCollider2D collider = transform.parent.GetComponent<BoxCollider2D>();
        transform.localScale = collider.size;
    }

}
