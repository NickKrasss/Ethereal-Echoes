using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUp : MonoBehaviour
{
    public GameObject GearPrefab;
    //Максимально число шестерёнок
    public int min_gears = 1;
    //Минимальное число шестерёнок 
    public int max_gears = 3;
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
                RandomGears();
            }
        }
    }

    public void RandomGears()
    {
        int gearCount = Random.Range(min_gears, max_gears + 1);
        for (int i = 0; i < gearCount; i++)
        {
            GameObject gear = Instantiate(GearPrefab, transform.position, Quaternion.identity);
            PickUp pickUp = gear.GetComponent<PickUp>();
            if (pickUp != null)
            {
                pickUp.id = 0; 
                pickUp.count = Random.Range(1, 4); 
            }
        }
    }

}
