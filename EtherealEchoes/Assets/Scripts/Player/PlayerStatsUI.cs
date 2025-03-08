using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private TMP_Text[] changeTexts;

    private TMP_Text[] statTexts;

    private float[] curStats;
    private float[] changes;
    private float[] cooldowns;

    private Stats playerStats;

    private float maxOpacity = 210;
    private float baseCooldown = 4f;

    private void Start()
    {
        curStats = new float[texts.Length];
        changes = new float[texts.Length];
        cooldowns = new float[texts.Length];

    }

    void Update()
    {
        if (playerStats == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerStats = player.GetComponent<Stats>();
                UpdateCurStats();
            }
            else
                return;
        }

        UpdateChanges();
        UpdateCurStats();
        UpdateChangeTexts();
        UpdateTexts();
        UpdateCooldowns();
    }

    void UpdateTexts()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = curStats[i].ToString("F2");
        }
    }

    void UpdateChangeTexts()
    {
        for (int i = 0; i < changeTexts.Length; i++)
        {
            if (changes[i] != 0)
            {
                changeTexts[i].text = (changes[i] > 0 ? "+  " : "-  ") + changes[i].ToString("F2");
                changeTexts[i].color = (changes[i] > 0 ? Color.green : Color.red);
                changeTexts[i].alpha = (cooldowns[i] > baseCooldown/2) ? (maxOpacity/255) : ((maxOpacity * (cooldowns[i]/(baseCooldown/2))) / 255);
                changeTexts[i].alpha = Mathf.Clamp(changeTexts[i].alpha, 0, maxOpacity/255);
            }
            else 
            {
                changeTexts[i].alpha = 0;
            }
        }
    }

    void UpdateChanges()
    {
        if (curStats[0] != playerStats.MoveSpeed)
        {
            changes[0] += playerStats.MoveSpeed - curStats[0];
            cooldowns[0] = baseCooldown;
        }
        if (curStats[1] != playerStats.Damage)
        {
            changes[1] += playerStats.Damage - curStats[1];
            cooldowns[1] = baseCooldown;
        }
        if (curStats[2] != playerStats.AttackSpeed)
        {
            changes[2] += playerStats.AttackSpeed - curStats[2];
            cooldowns[2] = baseCooldown;
        }
        if (curStats[3] != playerStats.AttackRange)
        {
            changes[3] += playerStats.AttackRange - curStats[3];
            cooldowns[3] = baseCooldown;
        }
        if (curStats[4] != playerStats.BulletSpeed)
        {
            changes[4] += playerStats.BulletSpeed - curStats[4];
            cooldowns[4] = baseCooldown;
        }
        if (curStats[5] != playerStats.Armor)
        {
            changes[5] += playerStats.Armor - curStats[5];
            cooldowns[5] = baseCooldown;
        }
    }

    void UpdateCurStats()
    {
        curStats[0] = playerStats.MoveSpeed;
        curStats[1] = playerStats.Damage;
        curStats[2] = playerStats.AttackSpeed;
        curStats[3] = playerStats.AttackRange;
        curStats[4] = playerStats.BulletSpeed;
        curStats[5] = playerStats.Armor;
    }

    void UpdateCooldowns()
    {
        for (int i = 0; i < cooldowns.Length; i++)
        {
            if (cooldowns[i] > 0)
            {
                cooldowns[i] -= Time.deltaTime;
                if (cooldowns[i] <= 0)
                {
                    cooldowns[i] = 0;
                    changes[i] = 0;
                }
            }
        }

    }

}
