using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PowerUpCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public string cardName;
    public string cardDescription;

    public List<CharacteristicUnit> valuesToChange;

    public Action actionOnMouseHover, actionOnClick;

    float value;
    float randomValue;

    [SerializeField] Image cardTint;
    [SerializeField] UnityEngine.Color idleColor, hoveredColor;
    bool isChangingColor;

    Coroutine changeColorCoroutine;

    private void Awake()
    {
        cardTint.color = idleColor;
    }

    void SetValue(CharacteristicUnit unit)
    {
        System.Reflection.PropertyInfo propName = Stats.Instance.GetType().GetProperty(unit.propertyName);

        if (unit.rangePercent.x != 0 && unit.rangePercent.y != 0)
            randomValue = UnityEngine.Random.Range(unit.rangePercent.x, unit.rangePercent.y + 1) / 100f + 1;
        else
            randomValue = UnityEngine.Random.Range(unit.rangeValue.x, unit.rangeValue.y + 1);

        if (propName != null)
        {
            if (unit.rangePercent.x != 0 && unit.rangePercent.y != 0)
                value = (float)((float)propName.GetValue(Stats.Instance) * randomValue);
            else
                value = (float)((float)propName.GetValue(Stats.Instance) + randomValue);

            propName.SetValue(Stats.Instance, value, null);
        }
        else
        {
            var fieldInfo = Stats.Instance.GetType().GetField(unit.propertyName);

            if (fieldInfo != null)
            {
                if (unit.rangePercent.x != 0 && unit.rangePercent.y != 0)
                    value = (float)((float)fieldInfo.GetValue(Stats.Instance) * randomValue);
                else
                    value = (float)((float)fieldInfo.GetValue(Stats.Instance) + randomValue);

                fieldInfo.SetValue(Stats.Instance, value);
            }
        }



        if (!string.IsNullOrEmpty(unit.propertyNameCurrent))
        {
            System.Reflection.PropertyInfo propNameCurrent = Stats.Instance.GetType().GetProperty(unit.propertyNameCurrent);

            if (propNameCurrent != null)
            {
                if (unit.rangePercent.x != 0 && unit.rangePercent.y != 0)
                    value = (float)((float)propNameCurrent.GetValue(Stats.Instance) * randomValue);
                else
                    value = (float)((float)propNameCurrent.GetValue(Stats.Instance) + randomValue);

                propNameCurrent.SetValue(Stats.Instance, value, null);
            }
            else
            {
                var fieldInfoCurrent = Stats.Instance.GetType().GetField(unit.propertyNameCurrent);

                if (fieldInfoCurrent != null)
                {
                    if (unit.rangePercent.x != 0 && unit.rangePercent.y != 0)
                        value = (float)((float)fieldInfoCurrent.GetValue(Stats.Instance) * randomValue);
                    else
                        value = (float)((float)fieldInfoCurrent.GetValue(Stats.Instance) + randomValue);

                    fieldInfoCurrent.SetValue(Stats.Instance, value);
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        actionOnMouseHover?.Invoke();

        if (isChangingColor)
            StopCoroutine(changeColorCoroutine);

        changeColorCoroutine = StartCoroutine(changeColor(hoveredColor, 0.15f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (CharacteristicUnit unit in valuesToChange)
        {
            SetValue(unit);
        }

        actionOnClick?.Invoke();
        InfoPopUpScreenController.Instance.Show("<size=15>Применено улучшение:</size>\n" + cardDescription, 5f);

        changeColorCoroutine = StartCoroutine(changeColor(hoveredColor, 0.15f, () =>
        {
            StartCoroutine(changeColor(idleColor, 0.15f, () =>
            {
                isChangingColor = false;
                changeColorCoroutine = null;
            }));
        }));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (changeColorCoroutine != null)
            StopCoroutine(changeColorCoroutine);

        if (isChangingColor)
            StopCoroutine(changeColorCoroutine);

        changeColorCoroutine = StartCoroutine(changeColor(idleColor, 0.15f));
    }

    IEnumerator changeColor(Color color, float duration, Action action = null)
    {
        float time = duration;
        Color startColor = cardTint.color;
        isChangingColor = true;

        while (time > 0f)
        {
            cardTint.color = Color.Lerp(startColor, color, 1 - time / duration);
            time -= Time.deltaTime;
            yield return null;
        }

        isChangingColor = false;
        cardTint.color = color;
        action?.Invoke();
    }
}

[System.Serializable]
public class CharacteristicUnit
{
    public string propertyName;
    public string propertyNameCurrent;
    [Space(20)]
    public int2 rangePercent;
    public int2 rangeValue;
}