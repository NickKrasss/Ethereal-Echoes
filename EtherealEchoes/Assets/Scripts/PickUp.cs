using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PickUp : MonoBehaviour
{
    public int id;
    [SerializeField] private float radius;
    [SerializeField] private float speed;
    public int count;
    Rigidbody2D rb;
    GameObject player;
    void Update()
    { 
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        if (Vector2.Distance(player.transform.position,transform.position)  < radius)
        {
            Vector2 direction = player.transform.position - transform.position;
            direction = direction.normalized;
            rb.AddForce(direction * speed * Time.deltaTime);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        }
    }

}
