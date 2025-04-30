using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GearContainer : MonoBehaviour
{
    public int current_gears;
    public int max_gears;
    public int value;
    public float Timer;
    public bool IsLevelingUp;
    public float coef = 1.1f;
    TMP_Text textMeshPro;
    TMP_Text upgradeText;

    [SerializeField]
    private AudioClip levelUpSound;

    [SerializeField]
    private float volume;

    public void AddGears(int gears)
    {
        if (current_gears  < max_gears)
        {
            current_gears += gears;
            if (current_gears > max_gears)
            {
                current_gears = max_gears;
            }
        }
    }

    private void Update()
    {
        if (!textMeshPro)
        {
            textMeshPro = GameObject.FindGameObjectWithTag("Gears").GetComponent<TMP_Text>();
            return;
        }
        if (!upgradeText)
        {
            upgradeText = GameObject.FindGameObjectWithTag("UpgradeText").GetComponent<TMP_Text>();
            return;
        }
        textMeshPro.text = $"{current_gears} / {max_gears}";
        upgradeText.text = IsFull() ? $"Зажмите <b><color=#8B0000>R</b></color> для повышения уровня." : "";
        LevelUp();
    }

    public void LevelUp()
    {
        float deltaTime = Time.deltaTime;
        if (current_gears >= max_gears)
        {
            if (Input.GetKey(KeyCode.R))
            {
                Timer += deltaTime;
                if (Timer > 1.5f)
                {
                    gameObject.GetComponent<Stats>().level++;
                    current_gears = 0;
                    max_gears += Mathf.CeilToInt(max_gears / (gameObject.GetComponent<Stats>().level * 2) * Mathf.Log(gameObject.GetComponent<Stats>().level + 1) * coef);
                    Timer = 0f;
                    if (AudioManager.Instance)
                        AudioManager.Instance.PlayAudio(levelUpSound, SoundType.SFX, volume);
                }
            }
            else
            {
                Timer = 0f;
            }
        }
    }
    public bool IsFull()
    {
        return current_gears == max_gears;
    }

}
