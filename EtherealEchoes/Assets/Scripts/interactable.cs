using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public bool Interact(GameObject interactor);
    public void SetHighlight(bool state);

    public GameObject GetGameObject();
}
