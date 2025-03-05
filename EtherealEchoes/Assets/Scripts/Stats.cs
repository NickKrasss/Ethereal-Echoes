using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // �������� ��������������

    [SerializeField] private float baseMaxHealth = 100f; // ������������ ��������

    public float BaseMaxHealth
    {
        get { return baseMaxHealth; }
        set { baseMaxHealth = value; }
    }

    public float MaxHealth 
    {
        get { return getValueAffectedByLevel(baseMaxHealth, level); }
    }

    public float CurrentHealth; // ������� ��������

    [SerializeField]  private float baseDamage = 10f; // ����
    public float BaseDamage
    {
        get { return baseDamage; }
        set { baseDamage = value; }
    }

    public float Damage
    {
        get { return getValueAffectedByLevel(baseDamage, level); }
    }
    [SerializeField] private float baseAttackSpeed = 1f; // �������� ����� (���� � �������)
    public float BaseAttackSpeed
    {
        get { return baseAttackSpeed; }
        set { baseAttackSpeed = value; }
    }

    public float AttackSpeed
    {
        get { return getValueAffectedByLevel(baseAttackSpeed, level); }
    }

    [SerializeField] private float baseMoveSpeed = 5f; // �������� ������������
    public float BaseMoveSpeed
    {
        get { return baseMoveSpeed; }
        set { baseMoveSpeed = value; }
    }

    public float MoveSpeed
    {
        get { return getValueAffectedByLevel(baseMoveSpeed, level); }
    }

    [SerializeField] private float baseMaxEnergy = 100f; // ������������ �������
    public float BaseMaxEnergy
    {
        get { return baseMaxEnergy; }
        set { baseMaxEnergy = value; }
    }

    public float MaxEnergy
    {
        get { return getValueAffectedByLevel(baseMaxEnergy, level); }
    }

    public float CurrentEnergy; // ������� �������

    [SerializeField] private float baseAttackRange = 10f; // ��������� �����
    public float BaseAttackRange
    {
        get { return baseAttackRange; }
        set { baseAttackRange = value; }
    }

    public float AttackRange
    {
        get { return getValueAffectedByLevel(baseAttackRange, level); }
    }

    [SerializeField] private float baseArmor; // ������� ������� �����
    public float BaseArmor
    {
        get { return baseArmor; }
        set { baseArmor = value; }
    }

    public float Armor
    {
        get { return getValueAffectedByLevel(baseArmor, level); }
    }

    public bool CanRegenerateHealth = true; // ����� �� ����������������� ��������
    public bool CanRegenerateEnergy = true; // ����� �� ����������������� �������
    public float HealthRegenerationRate = 1f; // �������� �������������� �������� � �������
    public float EnergyRegenerationRate = 2f; // �������� �������������� ������� � �������

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
}
