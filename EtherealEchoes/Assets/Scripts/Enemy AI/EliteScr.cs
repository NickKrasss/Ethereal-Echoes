using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class EliteScr : MonoBehaviour
{
    [SerializeField] public float chance;
    [SerializeField] private int levelBuff;
    [SerializeField] private float sizeMult;

    [SerializeField] private Material newMaterial;
    void Start()
    {
        if (Random.Range(0f, 100f) <= chance)
        {
            Stats stats = GetComponent<Stats>();
            stats.BaseMaxHealth *= 2.5f;
            stats.BaseDamage *= 2.5f;
            stats.CurrentHealth = GetComponent<Stats>().BaseMaxHealth;
            GetComponent<DropPickUp>().mult *= 2;
            GetComponent<Renderer>().material = newMaterial;
            GetComponent<MinimapIcon>().size *= sizeMult;
            transform.localScale = new Vector3(transform.localScale.x * sizeMult, transform.localScale.y * sizeMult, transform.localScale.z * sizeMult);
        }
    }
}
