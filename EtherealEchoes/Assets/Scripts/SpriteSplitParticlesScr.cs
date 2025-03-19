using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSplitParticlesScr : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] public float minLifetime;
    [SerializeField] public float maxLifetime;
    [SerializeField] public int minSize;
    [SerializeField] public int maxSize;
    [SerializeField] public int minCount;
    [SerializeField] public int maxCount;
    [SerializeField] public float minSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float gravity;

    [SerializeField] public Vector2 offset;
    [SerializeField] public float randOffset;

    public float objSize = 1;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CreateParticles(Vector2 position)
    {
        CreateParticles(position, Random.Range(minCount, maxCount));
    }

    public void CreateParticles(Vector2 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateParticle(position + new Vector2(offset.x + Random.Range(-randOffset, randOffset), offset.y + Random.Range(-randOffset, randOffset)), Random.Range(minLifetime, maxLifetime), Random.Range(minSize, maxSize), Random.Range(minSpeed, maxSpeed), gravity);
        }
    }

    private void CreateParticle(Vector2 position, float lifeTime, int size, float speed, float gravity)
    {
        GameObject particle = new GameObject("dads");
        particle.transform.position = position;
        SpriteRenderer particleRenderer = particle.AddComponent<SpriteRenderer>();
        particleRenderer.sprite = GenerateRandomSprite(size);
        particleRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        particle.AddComponent<DestroyAfterTime>().lifeTime = lifeTime;
        particle.transform.localScale = new Vector2(objSize, objSize);
        Rigidbody2D rb = particle.AddComponent<Rigidbody2D>();
        rb.drag = 0.2f;
        float rand = Random.Range(0, 2 * Mathf.PI);
        rb.velocity = new Vector2(Mathf.Cos(rand), Mathf.Sin(rand))*speed;
        rb.gravityScale = gravity;
        
    }

    private Sprite GenerateRandomSprite(int size, int attempt = 0)
    {
        if (attempt > 20) return null;

        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;

        int x = Random.Range(0, texture.width - size);
        int y = Random.Range(0, texture.height - size);

        Rect rect = new Rect(x, y, size, size);

        Sprite particleSprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), sprite.pixelsPerUnit);

        if (CheckIfRegionHasData(texture, rect))
            return particleSprite;
        else return GenerateRandomSprite(size, attempt+1);
    }

    private bool CheckIfRegionHasData(Texture2D texture, Rect rect, float alphaThreshold = 0.1f)
    {
        Color[] pixels = texture.GetPixels(
            (int)rect.x,
            (int)rect.y,
            (int)rect.width,
            (int)rect.height
        );

        foreach (Color pixel in pixels)
        {
            if (pixel.a > alphaThreshold)
            {
                return true;
            }
        }
        return false;
    }
}
