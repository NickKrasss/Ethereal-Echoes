using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Chest : MonoBehaviour, PurchasableItem, Interactable
{
    [SerializeField] public Animator animator;
    // Множитель
    public float mult;
    // Разброс
    [SerializeField] private int spread;
    // Базовая стоимость
    [SerializeField] private int basePrice;
    [SerializeField] private GameObject highlightUI;

    public int Price;
    // Открыт или нет
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
        Price = (int)((basePrice + Random.Range(-spread, spread)) * mult);
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
            animator.SetTrigger("Open");
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
                animator.ResetTrigger("Open");
                //Destroy(gameObject);
            }
        }
        return isOpened;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
