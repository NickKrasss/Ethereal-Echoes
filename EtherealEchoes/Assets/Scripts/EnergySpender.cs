using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class EnergySpender : MonoBehaviour
{

    [SerializeField] private Bar bar;
    private Stats stats;

    [SerializeField] private float cooldown;

    private float currentCooldown = 0f;

    void Start()
    {
        stats = GetComponent<Stats>();
        if (gameObject.CompareTag("Player"))
            bar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<Bar>();
    }

    void Update()
    {
        if (bar != null)
        {
            bar.SetMaxHP(stats.MaxEnergy);
            bar.SetValue(stats.CurrentEnergy / stats.MaxEnergy);
        }

        if (currentCooldown > 0)
        {
            stats.CanRegenerateEnergy = false;
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
                currentCooldown = 0;
        }
        else
        { 
            stats.CanRegenerateEnergy = true;
        }
    }

    public bool SpendEnergy(float energy)
    {
        if (stats.CurrentEnergy >= energy)
        { 
            stats.CurrentEnergy -= energy;
            currentCooldown = cooldown;
            return true;
        }
        if (bar) bar.Shake();
        return false;
    }
}
