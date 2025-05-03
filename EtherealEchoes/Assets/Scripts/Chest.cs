using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Chest : MonoBehaviour, PurchasableItem, Interactable
{
    [SerializeField] public Animator animator;
    //���������
    public float mult;
    //�������
    [SerializeField]private int spread;
    //������� ���������
    [SerializeField]private int basePrice;

    public int Price;
    //������ ��� ���
    public bool isOpened = false;
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
                Destroy(gameObject);
            }
        }
        return isOpened;
    }
    public void SetHighlight(bool state)
    {


    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
