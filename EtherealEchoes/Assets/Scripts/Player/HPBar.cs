using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private HealthScr healthScr;
    private RectTransform line;
    private RectTransform backLine;

    private Animation animation;

    [SerializeField]
    private float backLineSpeed = 5f;

    private void Start()
    {
        line = GetComponentsInChildren<Image>()[1].GetComponent<RectTransform>();
        backLine = GetComponentsInChildren<Image>()[0].GetComponent<RectTransform>();
        animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if (healthScr)
        {
            if (healthScr.hittedThatFrame)
                animation.Play();
            SetLine(healthScr.health / healthScr.maxHealth);
            SetBackLine(backLineSpeed * Time.deltaTime);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player"))
                healthScr = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScr>();
        }
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
