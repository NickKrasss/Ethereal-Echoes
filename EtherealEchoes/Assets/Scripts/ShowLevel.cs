using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class ShowLevel : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Color color;
    [SerializeField] private Color outlineColor;
    [SerializeField] private float outlineWidth;
    [SerializeField] private float fontSize;
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField] private int sortingOrder;

    [SerializeField] private Vector2 minMaxMouseRadius;

    private TextMeshPro tmp;
    private GameObject textObj;
    private Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();

        textObj = new GameObject(gameObject.name + "_lvlTextObject");
        textObj.transform.parent = G.Instance.currentWorldObj.transform;
        textObj.layer = 5;

        tmp = textObj.AddComponent<TextMeshPro>();
        textObj.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);

        tmp.color = color;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.font = fontAsset;
        tmp.sortingOrder = sortingOrder;
        tmp.sortingLayerID = SortingLayer.NameToID("Front");

        tmp.outlineColor = outlineColor;
        tmp.outlineWidth = outlineWidth;
    }

    private void Update()
    {
        if (textObj == null) return;
        textObj.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);
        tmp.text = $"{stats.level} lvl";

        float distance = Vector2.Distance((Vector2)WorldMousePosition.GetWorldMousePosition(Camera.main), (Vector2)transform.position);

        if (distance < minMaxMouseRadius.y)
            tmp.alpha = 1f;
        else if (distance > minMaxMouseRadius.x)
            tmp.alpha = 0f;
        else
            tmp.alpha = (minMaxMouseRadius.x - distance) / (minMaxMouseRadius.x - minMaxMouseRadius.y);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        Destroy(textObj);
    }
}
