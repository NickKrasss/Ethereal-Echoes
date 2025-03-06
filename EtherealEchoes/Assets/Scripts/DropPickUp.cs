using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUp : MonoBehaviour
{
    public GameObject GearPrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDestroy()
    {
        if (gameObject.scene.isLoaded) 
        {
            if (GearPrefab != null)
            {
                Instantiate(GearPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
