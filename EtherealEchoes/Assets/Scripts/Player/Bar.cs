using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private RectTransform line;
    private RectTransform backLine;

    private Animation anim;

    [SerializeField]
    private float backLineSpeed = 5f;

    private void Start()
    {
        line = GetComponentsInChildren<Image>()[2].GetComponent<RectTransform>();
        backLine = GetComponentsInChildren<Image>()[1].GetComponent<RectTransform>();
        anim = GetComponent<Animation>();
    }

    public void Shake()
    {
        anim.Play();
    }

    public void SetValue(float value)
    {
        SetLine(value);
        SetBackLine(backLineSpeed * Time.deltaTime);
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
