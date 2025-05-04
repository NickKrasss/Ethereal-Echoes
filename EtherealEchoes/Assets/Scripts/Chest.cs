using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


public class Chest : MonoBehaviour, PurchasableItem, Interactable
{
    [SerializeField] public string chestRarecy;

    [SerializeField] public int spread;
    [SerializeField] public int basePrice;
    [SerializeField] public GameObject highlightUI;

    [SerializeField]
    private TextMeshPro tmp;

    [HideInInspector]
    public int Price;
    [HideInInspector]
    public bool isOpened = false;

    [SerializeField]
    private GameObject top;

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

        Price = (int)(basePrice + (G.Instance.currentLevel-1) * 5 + Random.Range(-spread, spread)) * G.Instance.currentLevel;
        if (Price < 1)
        {
            Price = 1;
        }
        tmp.text = $"{Price} $";
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
    

    public bool Interact(GameObject interactor)
    {
        if(!isOpened)
        {
            if (Buy(interactor))
            {
                isOpened = true;
                tmp.gameObject.SetActive(false);
                top.SetActive(false);   

                if (chestRarecy == "common")
                {
                    G.Instance.powerUpCardsController.Initialize(G.Instance.dropChancesCommonChest);
                }

                if (chestRarecy == "rare")
                {
                    G.Instance.powerUpCardsController.Initialize(G.Instance.dropChancesRareChest);
                }

            }
        }
        return isOpened;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
