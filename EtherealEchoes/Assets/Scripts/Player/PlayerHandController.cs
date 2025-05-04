using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(FlipSpriteToMouse))]
[RequireComponent(typeof(PlayerGun))]
public class PlayerHandController : MonoBehaviour
{
    [SerializeField] private Transform hand1;
    [SerializeField] private Transform hand2;

    [SerializeField] private Transform gun1;
    [SerializeField] private Transform gun2;

    [SerializeField] private float rotationOffsetZ = -160f;

    private FlipSpriteToMouse flipScr;
    private PlayerGun playerGun;
    private void Start()
    {
        flipScr = GetComponent<FlipSpriteToMouse>();
        playerGun = GetComponent<PlayerGun>();
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f) return;

        Vector2 mousePosition = WorldMousePosition.GetWorldMousePosition(Camera.main);

        Transform hand = flipScr.isFlipped ? hand2 : hand1;
        Transform gun = flipScr.isFlipped ? gun2 : gun1;

        playerGun.shootPivot = gun;

        Vector2 direction = (mousePosition - (Vector2)gun.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (flipScr.isFlipped)
        { 
            targetAngle *= -1f;
            targetAngle -= 90f;
        }

        hand.localRotation = Quaternion.Euler(0, 0, targetAngle + rotationOffsetZ);

    }
}
