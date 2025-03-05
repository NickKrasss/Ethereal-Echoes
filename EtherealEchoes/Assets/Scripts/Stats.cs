using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Основные характеристики

    [SerializeField] private float baseMaxHealth = 100f; // Максимальное здоровье

    public float BaseMaxHealth
    {
        get { return baseMaxHealth; }
        set { baseMaxHealth = value; }
    }

    public float MaxHealth 
    {
        get { return getValueAffectedByLevel(baseMaxHealth, level); }
    }

    public float CurrentHealth; // Текущее здоровье

    [SerializeField]  private float baseDamage = 10f; // Урон
    public float BaseDamage
    {
        get { return baseDamage; }
        set { baseDamage = value; }
    }

    public float Damage
    {
        get { return getValueAffectedByLevel(baseDamage, level); }
    }
    [SerializeField] private float baseAttackSpeed = 1f; // Скорость атаки (атак в секунду)
    public float BaseAttackSpeed
    {
        get { return baseAttackSpeed; }
        set { baseAttackSpeed = value; }
    }

    public float AttackSpeed
    {
        get { return getValueAffectedByLevel(baseAttackSpeed, level); }
    }

    [SerializeField] private float baseMoveSpeed = 5f; // Скорость передвижения
    public float BaseMoveSpeed
    {
        get { return baseMoveSpeed; }
        set { baseMoveSpeed = value; }
    }

    public float MoveSpeed
    {
        get { return getValueAffectedByLevel(baseMoveSpeed, level); }
    }

    [SerializeField] private float baseMaxEnergy = 100f; // Максимальная энергия
    public float BaseMaxEnergy
    {
        get { return baseMaxEnergy; }
        set { baseMaxEnergy = value; }
    }

    public float MaxEnergy
    {
        get { return getValueAffectedByLevel(baseMaxEnergy, level); }
    }

    public float CurrentEnergy; // Текущая энергия

    [SerializeField] private float baseAttackRange = 10f; // Дальность атаки
    public float BaseAttackRange
    {
        get { return baseAttackRange; }
        set { baseAttackRange = value; }
    }

    public float AttackRange
    {
        get { return getValueAffectedByLevel(baseAttackRange, level); }
    }

    [SerializeField] private float baseArmor; // Текущий уровень брони
    public float BaseArmor
    {
        get { return baseArmor; }
        set { baseArmor = value; }
    }

    public float Armor
    {
        get { return getValueAffectedByLevel(baseArmor, level); }
    }

    public bool CanRegenerateHealth = true; // Может ли восстанавливаться здоровье
    public bool CanRegenerateEnergy = true; // Может ли восстанавливаться энергия
    public float HealthRegenerationRate = 1f; // Скорость восстановления здоровья в секунду
    public float EnergyRegenerationRate = 2f; // Скорость восстановления энергии в секунду

    public int level = 1;


    void Update()
    {
        RegenerateHealth();
        RegenerateEnergy();
    }

    public static float getValueAffectedByLevel(float value, int level)
    {
        return value * (1 + 0.02f * level);
    }

    public float GetDamageTaken(float incomingDamage)
    { 
        return CalculateDamageAfterArmor(incomingDamage);
    }

    //Уменьшение ХП в зависимости от брони
    private float CalculateDamageAfterArmor(float incomingDamage)
    {
        float maxArmor = 100f; 
        float damageReduction = Mathf.Log(Armor + 1) / Mathf.Log(maxArmor + 1);
        float finalDamage = incomingDamage * (1 - damageReduction);
        return Mathf.Max(finalDamage, 0);
    }  

    //Восстановление ХП в зависимости от карточек
    private void RegenerateHealth()
    {
        if (CanRegenerateHealth && CurrentHealth < MaxHealth)
        {
            CurrentHealth += HealthRegenerationRate * Time.deltaTime;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }
    }
    //Восстановление энергии в зависимости от карточек
    private void RegenerateEnergy()
    {
        if (CanRegenerateEnergy && CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += EnergyRegenerationRate * Time.deltaTime;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        }
    }
}
