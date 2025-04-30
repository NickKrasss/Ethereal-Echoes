using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class MinimapScr : MonoBehaviour
{
    [SerializeField] private GameObject minimapLayer;

    [SerializeField] private Color walkableColor;
    [SerializeField] private Color notWalkableColor;

    private WorldObject worldObj;
    private void Start()
    {
        SettingsManager.instance.onResolutionChanged += Resize;
    }
    private void OnDisable()
    {
        SettingsManager.instance.onResolutionChanged -= Resize;
    }

    public void GenerateTexture()
    {
        worldObj = G.Instance.currentWorldObj.GetComponent<WorldObject>();
        int width = worldObj.world.Width;
        int height = worldObj.world.Height;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, worldObj.world.Map[x, y] != 1 ? walkableColor : notWalkableColor);
            }
        }
        texture.Apply();
        minimapLayer.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), pixelsPerUnit: 1);
        SetSize();
    }

    [ContextMenu("SetSize")]
    void SetSize()
    {
        minimapLayer.transform.position = new Vector2(worldObj.world.Width / 2, worldObj.world.Height / 2);
        var parent = minimapLayer.transform.parent;
        minimapLayer.transform.localScale = new Vector2(1080f / Screen.currentResolution.height, 1080f / Screen.currentResolution.height);
    }
    void Resize()
    {
        StopAllCoroutines();
        StartCoroutine(reSize());
    }
    IEnumerator reSize()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SetSize();
    }
}
