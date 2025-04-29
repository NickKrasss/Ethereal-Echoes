using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(EnergySpender))]
public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private GameObject bulletPrefab;

    private float reload = 0f;

    [SerializeField]
    private AudioClip[] shootAudioClips;
    [SerializeField]
    private float shootVolume;

    private Stats stats;

    [SerializeField] private float rangeInaccuracy = 2;

    [SerializeField] private float attackEnergyCost;

    private EnergySpender energySpender;

    [HideInInspector] public Transform shootPivot;

    private void Start()
    {
        stats = GetComponent<Stats>();
        energySpender = GetComponent<EnergySpender>();
        G.Instance.playerObj = gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && reload <= 0f && energySpender.SpendEnergy(attackEnergyCost))
        {
            Shoot();    
        }
        if (reload > 0f)
            reload -= Time.deltaTime;
        else if (reload < 0f) reload = 0f;
    }

    private void Shoot()
    { 
        GameObject bullet = Instantiate(bulletPrefab, shootPivot.position, Quaternion.identity);
        if (G.Instance.powerUpCards.Contains("—Ì‡ÈÔÂ"))
        {
            int probability = UnityEngine.Random.Range(0, 100);
            if (probability <= G.Instance.criticalHitChance)
            {
                bullet.GetComponent<DamageHitBoxScr>().damage = (float)(stats.Damage * G.Instance.criticalHitAmount);
                //Debug.Log($"√Œ…ƒ¿ {(float)(stats.Damage * 1.75)}");
            }
        }
        else
        {
            bullet.GetComponent<DamageHitBoxScr>().damage = stats.Damage;
        }
        
        bullet.GetComponent<DamageHitBoxScr>().knockbackForce = stats.Knockback;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();
        scr.targetMoveVector = (WorldMousePosition.GetWorldMousePosition(camera) - shootPivot.position).normalized * stats.BulletSpeed;
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
            AudioManager.Instance.PlayAudio(shootAudioClips[UnityEngine.Random.Range(0, shootAudioClips.Length)], SoundType.SFX, shootVolume, 0.01f, 0.05f);
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

