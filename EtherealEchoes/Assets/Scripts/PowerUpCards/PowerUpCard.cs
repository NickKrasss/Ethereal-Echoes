﻿using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Reflection.Emit;

public class PowerUpCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public string cardName;
    public string cardDescription;

    // properties to change 
    public List<CharacteristicUnit> valuesToChange;
    public bool isCardOrbEffect;

    // actions
    public Action actionOnMouseHover, actionOnClick;

    // temp variables
    float value;
    float endValue;
    float randomValue;

    // UI elements
    [SerializeField] Image cardTint;
    [SerializeField] UnityEngine.Color idleColor, hoveredColor;

    // coroutine variables
    bool isChangingColor;
    Coroutine changeColorCoroutine;

    public void Awake()
    {
        // set the initial color of the card
        cardTint.color = idleColor;
    }

    

    void SetValue(CharacteristicUnit unit)
    {
        var player = G.Instance.playerObj;

        if (player == null) return;

        // get the property name from the Stats instance
        System.Reflection.PropertyInfo propName = player.GetComponent<Stats>().GetType().GetProperty(unit.propertyName);

        // generate a random value based on the specified range
        if (unit.rangePercent != 0)
            value = unit.rangePercent / 100f + 1;
        else
            value = unit.rangeValue;


        // if required characteristic is a property then edit as property
        if (propName != null)
        {
            if (unit.rangePercent != 0 )
                endValue = (float)((float)propName.GetValue(player.GetComponent<Stats>()) * value);
            else
                endValue = (float)((float)propName.GetValue(player.GetComponent<Stats>()) + value);

            // set the new value
            propName.SetValue(player.GetComponent<Stats>(), endValue, null);
        }
        else
        {
            // if required characteristic is a field then edit as field
            var fieldInfo = player.GetComponent<Stats>().GetType().GetField(unit.propertyName);

            if (fieldInfo != null)
            {
                if (unit.rangePercent != 0)
                    endValue = (float)((float)fieldInfo.GetValue(player.GetComponent<Stats>()) * value);
                else
                    endValue = (float)((float)fieldInfo.GetValue(player.GetComponent<Stats>()) + value);

                // set the new value
                fieldInfo.SetValue(player.GetComponent<Stats>(), endValue);
            }
        }

        // if the current property name is not empty, set the value for the current property
        // same implementation as above (maybe refactor this in the future)
        if (!string.IsNullOrEmpty(unit.propertyNameCurrent))
        {
            System.Reflection.PropertyInfo propNameCurrent = player.GetComponent<Stats>().GetType().GetProperty(unit.propertyNameCurrent);

            if (propNameCurrent != null)
            {
                if (unit.rangePercent != 0 )
                    endValue = (float)((float)propNameCurrent.GetValue(player.GetComponent<Stats>()) * value);
                else
                    endValue = (float)((float)propNameCurrent.GetValue(player.GetComponent<Stats>()) + value);

                propNameCurrent.SetValue(player.GetComponent<Stats>(), endValue, null);
            }
            else
            {
                var fieldInfoCurrent = player.GetComponent<Stats>().GetType().GetField(unit.propertyNameCurrent);

                if (fieldInfoCurrent != null)
                {
                    if (unit.rangePercent != 0 )
                        endValue = (float)((float)fieldInfoCurrent.GetValue(player.GetComponent<Stats>()) * value);
                    else
                        endValue = (float)((float)fieldInfoCurrent.GetValue(player.GetComponent<Stats>()) + value);

                    fieldInfoCurrent.SetValue(player.GetComponent<Stats>(), endValue);
                }
            }
        }
    }

    // this method is called when the mouse enters the card
    public void OnPointerEnter(PointerEventData eventData)
    {
        // invoke the action on mouse hover
        actionOnMouseHover?.Invoke();

        if (isChangingColor)
            StopCoroutine(changeColorCoroutine);

        // start the color change coroutine
        changeColorCoroutine = StartCoroutine(changeColor(hoveredColor, 0.15f));
    }

    static int countEqualsStringNames(List<string> stringNames, string name)
    {
        int count = 0;
        foreach (var names in stringNames)
        {
            if (names == name)
            {
                count++;
            }
        }

        return count;
    }

    // this method is called when the mouse clicks on the card
    public void OnPointerClick(PointerEventData eventData)
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (isCardOrbEffect)
        {
            if (cardName == "Ученый")
            {
                
                if (countEqualsStringNames(G.Instance.powerUpCards, "Ученый") == 0)
                {
                    G.Instance.extraGearsOffset += (float)(0.15);
                }
                else
                {
                    G.Instance.extraGearsOffset *= (float)(1.15);
                }
                G.Instance.powerUpCards.Add("Ученый");
                return;
            }
            if (cardName == "Снайпер")
            {
                if (countEqualsStringNames(G.Instance.powerUpCards, "Снайпер") > 1)
                {
                    G.Instance.criticalHitChance += (float)(G.Instance.criticalHitChance * 0.2);
                    G.Instance.powerUpCards.Add("Снайпер");
                }
                else if (countEqualsStringNames(G.Instance.powerUpCards, "Снайпер") == 0)
                {
                    G.Instance.criticalHitChance += 20;
                    G.Instance.powerUpCards.Add("Снайпер");
                }
                return;
            }
            if (cardName == "Карманник")
            {
                if (countEqualsStringNames(G.Instance.powerUpCards, "Карманник") == 0)
                {
                    G.Instance.extraGearsOffset += (float)(0.3);
                }
                else
                {
                    G.Instance.extraGearsOffset *= (float)(1.3);
                }
                G.Instance.powerUpCards.Add("Карманник");
                return;
            }
            if (cardName == "Фортуна")
            {
                for (var i = 0; i <= G.Instance.dropChancesCommonChest.Count; i++)
                {
                    G.Instance.dropChancesCommonChest[i] -= 4;
                    G.Instance.dropChancesRareChest[i] -= 4;
                    G.Instance.dropChancesStatsPlace[i] -= 4;
                    G.Instance.dropChancesArtifactPlace[i] -= 4;
                }
                G.Instance.powerUpCards.Add("Фортуна");
                return;
            }
            if (cardName == "Невосприимчивость")
            {
                if (G.Instance.blockDamageChance == 0)
                {
                    G.Instance.blockDamageChance += 7;
                }
                else
                {
                    G.Instance.blockDamageChance += (float)(G.Instance.blockDamageChance * 0.07);
                }
            }
            if (cardName == "Орлиный глаз")
            {
                G.Instance.criticalHitAmount += G.Instance.criticalHitAmount * 0.20;
            }

        }
        // set the value for each characteristic unit
        foreach (CharacteristicUnit unit in valuesToChange)
        {
            SetValue(unit);
        }
        G.Instance.powerUpCards.Add(cardName);

        // invoke the action on click
        actionOnClick?.Invoke();

        // show the info pop up with the card description
        InfoPopUpScreenController.Instance.Show("<size=0.75em>Применено улучшение:</size>\n" + cardDescription, 5f, 1f);

        // start the color change coroutine to change back to idle color
        changeColorCoroutine = StartCoroutine(changeColor(hoveredColor, 0.15f, () =>
        {
            StartCoroutine(changeColor(idleColor, 0.15f));
        }));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (changeColorCoroutine != null)
            StopCoroutine(changeColorCoroutine);

        if (isChangingColor)
            StopCoroutine(changeColorCoroutine);

        // start the color change coroutine to change back to idle color
        changeColorCoroutine = StartCoroutine(changeColor(idleColor, 0.15f));
    }

    // this method is called to change the color of the card
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

        // invoke the action if provided
        action?.Invoke();
    }
}

[System.Serializable]
public class CharacteristicUnit
{
    public string propertyName;
    public string propertyNameCurrent;
    public bool isCardOrbEffect;

    [Space(20)]
    public int rangePercent; // range in percent
    public float rangeValue; // range in units
}

// TODO: Enable the powerup to change parameters outside of Stats
// TODO: Add special effetcs for the powerup card
// TODO: Show card name on the card