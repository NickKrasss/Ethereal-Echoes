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
    //Сила, с которой вылетают шестерёнки
    public float dropForce = 0.5f;
    //Минимальное число-значение шестерёнок
    public int min_count;
    //Максимальное число-значение шестерёнок
    public int max_count;
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
                pickUp.count = Random.Range(min_count, max_count);
            }
            Rigidbody2D rb = gear.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float angle = Random.Range(0, 2 * Mathf.PI); 
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); 
                rb.AddForce(direction * dropForce, ForceMode2D.Impulse);
            }
        }
    }
} 
