using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject bulletPrefab;

    private float reload = 0f;

    private AudioSource auSource;

    [SerializeField]
    private AudioClip[] shootAudioClips;

    private Stats stats;

    [SerializeField] private float rangeInaccuracy = 2;

    private void Start()
    {
        auSource = GetComponent<AudioSource>();
        stats = GetComponent<Stats>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && reload <= 0f)
        {
            Shoot();    
        }
        if (reload > 0f)
            reload -= Time.deltaTime;
        else if (reload < 0f) reload = 0f;
    }

    private void Shoot()
    { 
        GameObject bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y+1), Quaternion.identity);
        bullet.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();
        scr.targetMoveVector = (WorldMousePosition.GetWorldMousePosition(camera) - new Vector3(transform.position.x, transform.position.y + 1, transform.position.z)).normalized * stats.BulletSpeed;
        float spread = UnityEngine.Random.Range(-stats.SpreadDegrees/2, stats.SpreadDegrees / 2) * Mathf.Deg2Rad;
        float x = scr.targetMoveVector.x;
        float y = scr.targetMoveVector.y;
        scr.targetMoveVector = new Vector2(x * Mathf.Cos(spread) - y * Mathf.Sin(spread), x * Mathf.Sin(spread) + y * Mathf.Cos(spread));
        reload = 1 / stats.AttackSpeed;

        DestroyAtRange rangeScr = bullet.GetComponent<DestroyAtRange>();
        if (rangeScr)
        { 
            rangeScr.range = stats.AttackRange + UnityEngine.Random.Range(-rangeInaccuracy/2, rangeInaccuracy/2);
        }
        if (AudioManager.Instance)
            AudioManager.Instance.PlayAudio(auSource, shootAudioClips[UnityEngine.Random.Range(0, shootAudioClips.Length)], SoundType.SFX, 0.15f, 0.05f, 0.1f);
    }
}

public static class WorldMousePosition
{
    public static Vector3 GetWorldMousePosition(Camera camera)
    {
        Ray cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
        float rayDistanceToZ0 = CalculateDistanceToZ0(cameraToMouseRay.origin, cameraToMouseRay.direction);
        return cameraToMouseRay.GetPoint(rayDistanceToZ0);
    }

    static float CalculateDistanceToZ0(Vector3 origin, Vector3 direction)
    {
        if (Mathf.Abs(direction.z) < Mathf.Epsilon)
        {
            return -1;
        }

        return -origin.z / direction.z;
    }
}

