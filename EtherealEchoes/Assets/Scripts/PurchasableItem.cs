using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  PurchasableItem 
{

    public bool Buy(GameObject interactor);

    public int GetPrice();
}
