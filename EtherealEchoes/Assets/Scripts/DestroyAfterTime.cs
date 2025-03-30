using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime;
    private float currentTime = 0f;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if ( currentTime >= lifeTime )
            Destroy(gameObject);
    }
}
