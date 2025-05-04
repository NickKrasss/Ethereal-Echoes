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
    // ����� ����� �����
    public float totalLifetime = 20f;
    // ������� ����� �����
    private float currentLifetime = 0f;
    // ����� ������ ��������
    public float flickerStartTime = 15f;
    // �������� ��������
    public float blinkInterval = 0.2f;
    // ����� ���������� ��������
    private float nextBlinkTime = 0f;
    // ������� ���������
    private bool isVisible = true;
    // ��������� ��� ���������� ����������
    private SpriteRenderer renderer;
    [SerializeField] private float radius;
    //�������� PickUp�
    public float speed;
    public int count;
    //������� Y ��� PickUp�
    [SerializeField] private float OffsetY;
    Rigidbody2D rb;
    GameObject player;

    [SerializeField]
    private AudioClip[] pickupSounds;
    [SerializeField]
    private float volume;
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

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (id == 0)
            {
                if (collision.gameObject.GetComponent<GearContainer>().IsFull()) return;
                if (G.Instance.extraGearsOffset > 0)
                {
                    var newGearCount = (int)(count + (count * G.Instance.extraGearsOffset));
                    collision.gameObject.GetComponent<GearContainer>().AddGears(newGearCount);
                }
                else
                {
                    collision.gameObject.GetComponent<GearContainer>().AddGears(count);
                }
                if (AudioManager.Instance)
                    AudioManager.Instance.PlayAudio(pickupSounds[UnityEngine.Random.Range(0, pickupSounds.Length)], SoundType.SFX, volume);
            }
            else if (id == 1)
            {
                if (collision.gameObject.GetComponent<Stats>().CurrentHealth >= collision.gameObject.GetComponent<Stats>().MaxHealth) return;
                collision.gameObject.GetComponent<Stats>().AddHp(count);
                if (AudioManager.Instance)
                    AudioManager.Instance.PlayAudio(pickupSounds[UnityEngine.Random.Range(0, pickupSounds.Length)], SoundType.SFX, volume);
            }
            else if (id == 2)
            {
                if (collision.gameObject.GetComponent<Stats>().CurrentEnergy >= collision.gameObject.GetComponent<Stats>().MaxEnergy) return;
                collision.gameObject.GetComponent<Stats>().AddEnergy(count);
                if (AudioManager.Instance)
                    AudioManager.Instance.PlayAudio(pickupSounds[UnityEngine.Random.Range(0, pickupSounds.Length)], SoundType.SFX, volume);
            }
            Destroy(gameObject);
        }  
    }

}
