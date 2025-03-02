using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // �������� ��������������
    public float MaxHealth = 100f; // ������������ ��������
    public float CurrentHealth; // ������� ��������
    public float Damage = 10f; // ����
    public float AttackSpeed = 1f; // �������� ����� (���� � �������)
    public float MoveSpeed = 5f; // �������� ������������
    public float MaxEnergy = 100f; // ������������ �������
    public float CurrentEnergy; // ������� �������
    public float AttackRange = 2f; // ��������� �����
    public float CurrentArmor; // ������� ������� �����
    public bool CanRegenerateHealth = true; // ����� �� ����������������� ��������
    public bool CanRegenerateEnergy = true; // ����� �� ����������������� �������
    public float HealthRegenerationRate = 1f; // �������� �������������� �������� � �������
    public float EnergyRegenerationRate = 2f; // �������� �������������� ������� � �������
    void Update()
    {
        RegenerateHealth();
        RegenerateEnergy();
    }
    // ��������� ����� � ������ �����
    public void TakeDamage(float incomingDamage)
    {
        float damageAfterArmor = CalculateDamageAfterArmor(incomingDamage);
        CurrentHealth -= damageAfterArmor;
        if (CurrentHealth <= 0)
        {
           //
        }
    }   
    //���������� �� � ����������� �� �����
    private float CalculateDamageAfterArmor(float incomingDamage)
    {
        float maxArmor = 100f; 
        float damageReduction = Mathf.Log(CurrentArmor + 1) / Mathf.Log(maxArmor + 1);
        float finalDamage = incomingDamage * (1 - damageReduction);
        return Mathf.Max(finalDamage, 0);
    }  
    //�������������� ��
    public void HpHeal()
    {
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        RegenerateHealth();
    }
    //����� �������
    public void SpendEnergy(float energyCost)
    {
        CurrentEnergy -= energyCost;
        if (CurrentEnergy < 0)
        {
            CurrentEnergy = 0;
        }
    }

    //�������������� �������
    public void RegenerationEnergy()
    {
        if (CurrentEnergy >= MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
        RegenerateEnergy();
    }
    //���������� �����
    public void addArmor(float armor)
    {
        CurrentArmor += armor;
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
