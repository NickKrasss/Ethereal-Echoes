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
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<DamageHitBoxScr>().damage = bulletDamage;
        SmoothMoveScr scr = bullet.GetComponent<SmoothMoveScr>();

        Ray cameraToMouseRay = camera.ScreenPointToRay(Input.mousePosition);
        float rayDistanceToZ0 = CalculateDistanceToZ0(cameraToMouseRay.origin, cameraToMouseRay.direction);
        scr.targetMoveVector = (cameraToMouseRay.GetPoint(rayDistanceToZ0) - transform.position).normalized * bulletSpeed;
        reload = 1 / fireRate;
    }

    float CalculateDistanceToZ0(Vector3 origin, Vector3 direction)
    {
        if (Mathf.Abs(direction.z) < Mathf.Epsilon)
        {
            return -1;
        }

        return -origin.z / direction.z;
    }
}
