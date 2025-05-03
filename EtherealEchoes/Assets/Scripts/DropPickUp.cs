using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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
        if (G.Instance.isWorldLoading) return;
        int gearCount = Random.Range(min_gears, max_gears + 1) + G.Instance.currentLevel-1;
        for (int i = 0; i < gearCount; i++)
        {
            GameObject gear = Instantiate(GearPrefab, transform.position, Quaternion.identity);
            gear.transform.SetParent(G.Instance.currentWorldObj.transform);
            PickUp pickUp = gear.GetComponent<PickUp>();
            if (pickUp != null)
            {
                pickUp.id = 0;
                pickUp.count = Random.Range(min_count, max_count);
                float scaleFactor = 0.2f + (pickUp.count - 1) * 0.04f;
                gear.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
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
