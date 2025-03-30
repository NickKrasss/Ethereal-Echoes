using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // �������� ��������������

    [SerializeField] private float baseMaxHealth = 100f; // ������������ ��������
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
    public float CurrentHealth; // ������� ��������

    [SerializeField]  private float baseDamage = 10f; // ����
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
    [SerializeField] private float baseAttackSpeed = 1f; // �������� ����� (���� � �������)
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

    [SerializeField] private float baseMoveSpeed = 5f; // �������� ������������
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

    [SerializeField] private float baseMaxEnergy = 100f; // ������������ �������
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

    public float CurrentEnergy; // ������� �������

    [SerializeField] private float baseAttackRange = 10f; // ��������� �����
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

    [SerializeField] private float baseArmor; // ������� ������� �����
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

    public bool CanRegenerateHealth = true; // ����� �� ����������������� ��������
    public bool CanRegenerateEnergy = true; // ����� �� ����������������� �������
    public float HealthRegenerationRate = 1f; // �������� �������������� �������� � �������
    public float EnergyRegenerationRate = 2f; // �������� �������������� ������� � �������

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

    //���������� �� � ����������� �� �����
    private float CalculateDamageAfterArmor(float incomingDamage)
    {
        float maxArmor = 100f; 
        float damageReduction = Mathf.Log(Armor + 1) / Mathf.Log(maxArmor + 1);
        float finalDamage = incomingDamage * (1 - damageReduction);
        return Mathf.Max(finalDamage, 0);
    }  

    //�������������� �� � ����������� �� ��������
    private void RegenerateHealth()
    {
        if (CanRegenerateHealth && CurrentHealth < MaxHealth)
        {
            CurrentHealth += HealthRegenerationRate * Time.deltaTime;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }
    }
    //�������������� ������� � ����������� �� ��������
    private void RegenerateEnergy()
    {
        if (CanRegenerateEnergy && CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += EnergyRegenerationRate * Time.deltaTime;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        }
    }
    //���������� �� ��� PickUp�
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
    //���������� ������� ��� PickUp�
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
