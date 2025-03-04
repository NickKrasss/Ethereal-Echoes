using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PickUp : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private float radius;
    [SerializeField] private float speed;
    [SerializeField] private int count;
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


}
