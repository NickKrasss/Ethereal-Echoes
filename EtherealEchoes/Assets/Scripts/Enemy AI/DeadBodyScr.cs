using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DeadBodyScr : MonoBehaviour
{
    private Transform[] parts;

    [SerializeField] public float fallSpeed;
    [SerializeField] public float spreadForce;
    [SerializeField] public float spreadForceRandomOffset;

    public bool flip = false;

    private void Start()
    {
        parts = transform.GetComponentsInChildren<Transform>()[1..];
        SpriteRenderer sprRenderer = GetComponent<SpriteRenderer>();
        if (sprRenderer) sprRenderer.flipX = flip;
        foreach (var part in parts)
        {
            Rigidbody2D rb = part.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.drag = 7f + Random.Range(-2f, 2f);
            rb.angularDrag = 7f + Random.Range(-2f, 2f);
            float rand = Random.Range(0, 2 * Mathf.PI);
            Vector2 direction = new Vector2(Mathf.Cos(rand), Mathf.Sin(rand));
            rb.AddForce(direction * (spreadForce + Random.Range(-spreadForceRandomOffset, spreadForceRandomOffset)), ForceMode2D.Impulse);
            rb.AddTorque((Random.Range(-spreadForce, spreadForce)), ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        FallUpdate();
    }

    private void FallUpdate()
    {
        transform.rotation = Quaternion.Euler(Mathf.LerpAngle(transform.rotation.eulerAngles.x, 0, Time.deltaTime * fallSpeed), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, 0, Time.deltaTime * fallSpeed));
        foreach (var part in parts)
        {
            part.transform.rotation = Quaternion.Euler(Mathf.LerpAngle(part.transform.rotation.eulerAngles.x, 0, Time.deltaTime * fallSpeed), part.transform.rotation.eulerAngles.y, part.transform.rotation.eulerAngles.z);
            part.transform.position = new Vector3(part.transform.position.x, part.transform.position.y, Mathf.Lerp(part.transform.position.z, 0, Time.deltaTime * fallSpeed));
        }
    }
}
