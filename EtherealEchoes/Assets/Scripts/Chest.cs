using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[RequireComponent(typeof(Animator))]

public class Chest : MonoBehaviour, PurchasableItem, Interactable
{
    [SerializeField] public string chestRarecy;
    [SerializeField] public Animator animator;
    [SerializeField] public float mult;
    [SerializeField] public int spread;
    [SerializeField] public int basePrice;
    [SerializeField] public GameObject highlightUI;

    [SerializeField] public Vector2 offset;
    [SerializeField] public UnityEngine.Color color;
    [SerializeField] public UnityEngine.Color outlineColor;
    [SerializeField] public float outlineWidth;
    [SerializeField] public float fontSize;
    [SerializeField] public TMP_FontAsset fontAsset;

    private GameObject textObj;
    private TextMeshPro tmp;

    public int Price;
    // Открыт или нет
    public bool isOpened = false;

    public void MakeText()
    {
        textObj = new GameObject(gameObject.name + "_lvlTextObject");
        textObj.transform.parent = G.Instance.currentWorldObj.transform;
        textObj.layer = 5;

        tmp = textObj.AddComponent<TextMeshPro>();
        textObj.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);

        tmp.color = color;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.font = fontAsset;
        //tmp.sortingOrder = sortingOrder;
        tmp.sortingLayerID = SortingLayer.NameToID("Front");

        tmp.outlineColor = outlineColor;
        tmp.outlineWidth = outlineWidth;
    }

    private void Awake()
    {
        MakeText();
    }

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
        Price = (basePrice + Random.Range(-spread, spread));
        if (Price < 1)
        {
            Price = 1;
        }
    }

    private void Update()
    {

        //if (textObj == null) return;
        //textObj.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);
        //tmp.text = $"{Price}";

        //доделать текст

        
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
                animator.SetTrigger("Open");
                isOpened = true;
                //Destroy(gameObject);
                //animator.ResetTrigger("Open");

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

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        Destroy(textObj);
    }
}
