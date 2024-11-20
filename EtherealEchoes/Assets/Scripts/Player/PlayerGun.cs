using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed = 5f;

    [SerializeField]
    private float bulletDamage = 8;

    [SerializeField]
    private float bulletKnockback = 5f;

    [SerializeField]
    private float fireRate = 4;

    [SerializeField]
    private float spreadDegrees = 0;

    [SerializeField]
    private float range = 5;

    [SerializeField]
    private float rangeInaccuracy = 5;

    private float reload = 0f;


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
        bullet.GetComponent<DamageHitBoxScr>().damage = bulletDamage;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();
        scr.targetMoveVector = (WorldMousePosition.GetWorldMousePosition(camera) - new Vector3(transform.position.x, transform.position.y + 1, transform.position.z)).normalized * bulletSpeed;
        float spread = UnityEngine.Random.Range(-spreadDegrees/2, spreadDegrees/2) * Mathf.Deg2Rad;
        float x = scr.targetMoveVector.x;
        float y = scr.targetMoveVector.y;
        scr.targetMoveVector = new Vector2(x * Mathf.Cos(spread) - y * Mathf.Sin(spread), x * Mathf.Sin(spread) + y * Mathf.Cos(spread));
        reload = 1 / fireRate;

        DestroyAtRange rangeScr = bullet.GetComponent<DestroyAtRange>();
        if (rangeScr)
        { 
            rangeScr.range = range + UnityEngine.Random.Range(-rangeInaccuracy/2, rangeInaccuracy/2);
        }
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

