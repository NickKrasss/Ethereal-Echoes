﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AltarsScr : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action actionOnMouseHover, actionOnClick;
    [SerializeField] public string altarType;
    [SerializeField] public int spread;
    [SerializeField] public int basePrice;
    [SerializeField] public GameObject highlightUI;


    // äëÿ òåêñòà
    [SerializeField] public Vector2 offset;
    [SerializeField] public UnityEngine.Color color;
    [SerializeField] public UnityEngine.Color outlineColor;
    [SerializeField] public float outlineWidth;
    [SerializeField] public float fontSize;
    [SerializeField] public TMP_FontAsset fontAsset;

    public int Price;
    public bool isOpened = false;

    public void SetHighlight(bool state)
    {
        if (state)
        {
            highlightUI.SetActive(true);
        }
        else
        {
            highlightUI.SetActive(false);
        }
    }

    private void Start()
    {
        Price = (basePrice + UnityEngine.Random.Range(-spread, spread));
        if (Price < 1)
        {
            Price = 1;
        }
    }
    public int GetPrice()
    {
        return Price;
    }

    public bool Buy(GameObject interactor)
    {
        if (Price <= interactor.GetComponent<GearContainer>().current_gears)
        {
            interactor.GetComponent<GearContainer>().current_gears -= Price;

            return true;
        }
        return false;
    }

    // Executed when the mouse is over the altar
    public void OnPointerEnter(PointerEventData eventData)
    {
        actionOnMouseHover?.Invoke();

        InfoPopUpScreenController.Instance.Show("*текст для алтаря*", 5f);
    }
    // Executed when the mouse exits the altar
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPopUpScreenController.Instance.HidePopUpImmediately();
    }
    public bool Interact(GameObject interactor)
    {
        if (!isOpened)
        {
            if (Buy(interactor))
            {
                //animator.SetTrigger("Open");
                isOpened = true;
                //Destroy(gameObject);
                //animator.ResetTrigger("Open");

                if (altarType == "chest")
                {
                    G.Instance.powerUpCardsController.Initialize(G.Instance.dropChancesRareChest);
                }

                if (altarType == "maiden")
                {
                    G.Instance.playerObj.GetComponent<Stats>().CurrentHealth = G.Instance.playerObj.GetComponent<Stats>().CurrentHealth / 2;
                    G.Instance.playerObj.GetComponent<Stats>().BaseDamage += (float)1.5;
                }

                if (altarType == "book")
                {
                    G.Instance.powerUpCardsController.Initialize(G.Instance.dropChancesStatsPlace);
                }

                if (altarType == "heal")
                {
                    G.Instance.playerObj.GetComponent<Stats>().CurrentHealth = G.Instance.playerObj.GetComponent<Stats>().MaxHealth;
                    G.Instance.playerObj.GetComponent<Stats>().BaseMaxHealth += 12;
                }

            }
        }
        return isOpened;
    }
}
