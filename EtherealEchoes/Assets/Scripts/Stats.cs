using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Основные характеристики

    [SerializeField] private float baseMaxHealth = 100f; // Максимальное здоровье
    [SerializeField] private bool isMaxHealthAffectedByLevel = true;

    public float BaseMaxHealth
    {
        get { return baseMaxHealth; }
        set { baseMaxHealth = value; }
    }

    public float MaxHealth 
    {
        get 
        { 
            if (isMaxHealthAffectedByLevel)
                return getValueAffectedByLevel(baseMaxHealth, level);
            return baseMaxHealth;
        }
    }

    [HideInInspector]
    public float CurrentHealth; // Текущее здоровье

    [SerializeField]  private float baseDamage = 10f; // Урон
    [SerializeField] private bool isDamageAffectedByLevel = true;
    public float BaseDamage
    {
        get { return baseDamage; }
        set { baseDamage = value; }
    }

    public float Damage
    {
        get
        {
            if (isDamageAffectedByLevel)
                return getValueAffectedByLevel(baseDamage, level);
            return baseDamage;
        }
    }
    [SerializeField] private float baseAttackSpeed = 1f; // Скорость атаки (атак в секунду)
    [SerializeField] private bool isAttackSpeedAffectedByLevel = true;
    public float BaseAttackSpeed
    {
        get { return baseAttackSpeed; }
        set { baseAttackSpeed = value; }
    }

    public float AttackSpeed
    {
        get
        {
            if (isAttackSpeedAffectedByLevel)
                return getValueAffectedByLevel(baseAttackSpeed, level);
            return baseAttackSpeed;
        }
    }

    [SerializeField] private float baseMoveSpeed = 5f; // Скорость передвижения
    [SerializeField] private bool isMoveSpeedAffectedByLevel = true;
    public float BaseMoveSpeed
    {
        get { return baseMoveSpeed; }
        set { baseMoveSpeed = value; }
    }

    public float MoveSpeed
    {
        get
        {
            if (isMoveSpeedAffectedByLevel)
                return getValueAffectedByLevel(baseMoveSpeed, level);
            return baseMoveSpeed;
        }
    }

    [SerializeField] private float baseMaxEnergy = 100f; // Максимальная энергия
    [SerializeField] private bool isMaxEnergyAffectedByLevel = true;
    public float BaseMaxEnergy
    {
        get { return baseMaxEnergy; }
        set { baseMaxEnergy = value; }
    }

    public float MaxEnergy
    {
        get
        {
            if (isMaxEnergyAffectedByLevel)
                return getValueAffectedByLevel(baseMaxEnergy, level);
            return baseMaxEnergy;
        }
    }

    public float CurrentEnergy; // Текущая энергия

    [SerializeField] private float baseAttackRange = 10f; // Дальность атаки
    [SerializeField] private bool isAttackRangeAffectedByLevel = true;
    public float BaseAttackRange
    {
        get { return baseAttackRange; }
        set { baseAttackRange = value; }
    }

    public float AttackRange
    {
        get
        {
            if (isAttackRangeAffectedByLevel)
                return getValueAffectedByLevel(baseAttackRange, level);
            return baseAttackRange;
        }
    }

    [SerializeField] private float baseArmor; // Текущий уровень брони
    [SerializeField] private bool isArmorAffectedByLevel = true;
    public float BaseArmor
    {
        get { return baseArmor; }
        set { baseArmor = value; }
    }

    public float Armor
    {
        get
        {
            if (isArmorAffectedByLevel)
                return getValueAffectedByLevel(baseArmor, level);
            return baseArmor;
        }
    }

    [SerializeField] public float BulletSpeed = 16f;

    [SerializeField] public float Knockback = 5f;

    [SerializeField] public float SpreadDegrees = 0;

    public bool CanRegenerateHealth = true; // Может ли восстанавливаться здоровье
    public bool CanRegenerateEnergy = true; // Может ли восстанавливаться энергия
    public float HealthRegenerationRate = 1f; // Скорость восстановления здоровья в секунду
    public float EnergyRegenerationRate = 2f; // Скорость восстановления энергии в секунду

    public int level = 1;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        CurrentEnergy = MaxEnergy;
    }

    void Update()
    {
        RegenerateHealth();
        RegenerateEnergy();
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }

    public static float getValueAffectedByLevel(float value, int level)
    {
        return value * (1 + 0.035f * level);
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
    //Добавление ХП при PickUpе
    public void AddHp(int hp)
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += hp;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
    }
    //Добавление Энергии при PickUpе
    public void AddEnergy(int energy)
    {
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += energy;
            if (CurrentEnergy > MaxEnergy)
            {
                CurrentEnergy = MaxEnergy;
            }
        }
    }

}
