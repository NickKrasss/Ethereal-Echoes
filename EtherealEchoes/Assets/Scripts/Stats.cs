using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Основные характеристики
    public float MaxHealth = 100f; // Максимальное здоровье
    public float CurrentHealth; // Текущее здоровье
    public float Damage = 10f; // Урон
    public float AttackSpeed = 1f; // Скорость атаки (атак в секунду)
    public float MoveSpeed = 5f; // Скорость передвижения
    public float MaxEnergy = 100f; // Максимальная энергия
    public float CurrentEnergy; // Текущая энергия
    public float AttackRange = 2f; // Дальность атаки
    public float CurrentArmor; // Текущий уровень брони
    public bool CanRegenerateHealth = true; // Может ли восстанавливаться здоровье
    public bool CanRegenerateEnergy = true; // Может ли восстанавливаться энергия
    public float HealthRegenerationRate = 1f; // Скорость восстановления здоровья в секунду
    public float EnergyRegenerationRate = 2f; // Скорость восстановления энергии в секунду
    void Update()
    {
        RegenerateHealth();
        RegenerateEnergy();
    }
    // Получение урона с учетом брони
    public void TakeDamage(float incomingDamage)
    {
        float damageAfterArmor = CalculateDamageAfterArmor(incomingDamage);
        CurrentHealth -= damageAfterArmor;
        if (CurrentHealth <= 0)
        {
           //
        }
    }   
    //Уменьшение ХП в зависимости от брони
    private float CalculateDamageAfterArmor(float incomingDamage)
    {
        float maxArmor = 100f; 
        float damageReduction = Mathf.Log(CurrentArmor + 1) / Mathf.Log(maxArmor + 1);
        float finalDamage = incomingDamage * (1 - damageReduction);
        return Mathf.Max(finalDamage, 0);
    }  
    //Восстановление ХП
    public void HpHeal()
    {
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        RegenerateHealth();
    }
    //Трата энергии
    public void SpendEnergy(float energyCost)
    {
        CurrentEnergy -= energyCost;
        if (CurrentEnergy < 0)
        {
            CurrentEnergy = 0;
        }
    }

    //Восстановление энергии
    public void RegenerationEnergy()
    {
        if (CurrentEnergy >= MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
        RegenerateEnergy();
    }
    //Добавление брони
    public void addArmor(float armor)
    {
        CurrentArmor += armor;
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
