using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PickUp : MonoBehaviour
{
    public int id;
    // Общее время жизни
    public float totalLifetime = 20f;
    // Текущее время жизни
    private float currentLifetime = 0f;
    // Время начала мерцания
    public float flickerStartTime = 15f;
    // Интервал мерцания
    public float blinkInterval = 0.2f;
    // Время следующего мерцания
    private float nextBlinkTime = 0f;
    // Текущая видимость
    private bool isVisible = true;
    // Компонент для управления видимостью
    private SpriteRenderer renderer;
    [SerializeField] private float radius;
    //Скорость PickUpа
    public float speed;
    public int count;
    //Позиция Y для PickUpа
    [SerializeField] private float OffsetY;
    Rigidbody2D rb;
    GameObject player;
    void Update()
    {
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= totalLifetime)
        {
            Destroy(gameObject);
            return;
        }
        if (currentLifetime >= flickerStartTime)
        {
            float remainingTime = totalLifetime - currentLifetime;
            blinkInterval = Mathf.Lerp(0.05f, 0.2f, remainingTime / (totalLifetime - flickerStartTime));
            if (Time.time >= nextBlinkTime)
            {
                isVisible = ! isVisible;
                nextBlinkTime = Time.time + blinkInterval;
                renderer.enabled = isVisible;
            }
        }
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        Vector3 target = new Vector2(player.transform.position.x, player.transform.position.y - OffsetY);
        if (Vector2.Distance(target,transform.position)  < radius)
        {

            Vector2 direction = target  - transform.position;
            direction = direction.normalized;
            rb.AddForce(direction * speed * Time.deltaTime);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (id == 0)
            {
                collision.gameObject.GetComponent<GearContainer>().AddGears(count);
            }
            else if (id == 1)
            {
                collision.gameObject.GetComponent<Stats>().AddHp(count);
            }
            else if (id == 2)
            {
                collision.gameObject.GetComponent<Stats>().AddEnergy(count);
            }
            Destroy(gameObject);
        }  
    }

}
