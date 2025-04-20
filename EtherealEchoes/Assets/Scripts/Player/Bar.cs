using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private RectTransform line;
    [SerializeField] private RectTransform backLine;
    [SerializeField] private RectTransform barSlot;

    private float barSlotStartScale;
    private float barSlotStartWidth;

    [SerializeField] private float slotPerHP = 10f;

    private Animation anim;

    [SerializeField]
    private float backLineSpeed = 5f;

    [SerializeField] private GameObject gear;
    [SerializeField] private float gearSpeed;
    private float gearSpeedMult = 1;

    private void Start()
    {
/*        line = GetComponentsInChildren<Image>()[2].GetComponent<RectTransform>();
        backLine = GetComponentsInChildren<Image>()[1].GetComponent<RectTransform>();
        barSlot = GetComponentsInChildren<Image>()[3].GetComponent<RectTransform>();   
*/
        anim = GetComponent<Animation>();
        barSlotStartScale = barSlot.localScale.x;
        barSlotStartWidth = barSlot.sizeDelta.x;
    }

    private void Update()
    {
        if (gear != null)
        {
            gear.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * gearSpeed * gearSpeedMult));
        }
    }

    public void Shake()
    {
        anim.Play();
    }

    public void SetValue(float value)
    {
        SetLine(value);
        SetBackLine(backLineSpeed * Time.deltaTime);
        gearSpeedMult = 7.5f - value*5;
    }

    public void SetMaxHP(float maxHP)
    {
        float slots = maxHP / slotPerHP;
        barSlot.localScale = new Vector2(barSlotStartScale / slots, barSlot.localScale.y);
        barSlot.sizeDelta = new Vector2(barSlotStartWidth * slots, barSlot.sizeDelta.y);
    }

    private void SetLine(float x)
    {
        line.localScale = new Vector3(x, 1);
    }

    private void SetBackLine(float speed)
    {
        float current = backLine.localScale.x;
        float target = line.localScale.x;
        backLine.localScale = new Vector3(
            Mathf.Lerp(
                current,
                target, 
                speed * (Mathf.Abs(current-target) + 0.5f)
                ),
            1);
    }
}
