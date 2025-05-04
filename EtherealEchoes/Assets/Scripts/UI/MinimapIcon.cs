using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [SerializeField] private Sprite iconSpr;
    [SerializeField] public float size;
    public GameObject icon;
    void Start()
    {
        icon = new GameObject($"{gameObject.name}_icon");
        icon.transform.parent = transform;
        icon.transform.position = transform.position;
        SpriteRenderer iconRenderer = icon.AddComponent<SpriteRenderer>();
        iconRenderer.sprite = iconSpr;
        iconRenderer.sortingOrder = 1;
        icon.layer = 8;
        icon.transform.localScale = new Vector2(3 * size, 3 * size);
    }
}
