using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationByMouseScr : MonoBehaviour
{
    // ������� ������ � ������ ������
    private Vector2 currentRotation;

    private Vector2 targetRotation;

    [Tooltip("���� \"������\". ��� ������ - ��� �������")]
    [SerializeField]
    private float lerpSpeed = 1.0f;

    [Tooltip("����������� �������� �� ����. ��� ������ - ��� ������� ������ ��������������")]
    [SerializeField]
    private float mouseForce = 1.0f;

    private float cameraShakeForce;

    private void Awake()
    {
        StartCoroutine(UpdateShakeForce());
    }

    IEnumerator UpdateShakeForce()
    {
        cameraShakeForce = PlayerPrefs.GetFloat("CameraShakeForce", 1.0f);
        yield return new WaitForSeconds(2);
        StartCoroutine(UpdateShakeForce());
    }

    // ���������� currentRotation � targetRotation
    private void UpdateCurrent()
    {
        float x = Mathf.Lerp(currentRotation.x, targetRotation.x, lerpSpeed * Time.deltaTime);
        float y = Mathf.Lerp(currentRotation.y, targetRotation.y, lerpSpeed * Time.deltaTime);
        currentRotation = new Vector2(x, y);
    }

    // ��������� targetRotation
    private void UpdateTarget()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x - Camera.main.scaledPixelWidth/2, Input.mousePosition.y - Camera.main.scaledPixelHeight / 2);
        targetRotation = new Vector2(cameraShakeForce * mouseForce * (mousePos.x / Camera.main.scaledPixelWidth > 1 ? 1 : mousePos.x / Camera.main.scaledPixelWidth), cameraShakeForce * mouseForce * (mousePos.y / Camera.main.scaledPixelHeight > 1 ? 1 : mousePos.y / Camera.main.scaledPixelHeight));
    }

    private void Update()
    {
        UpdateCurrent();
        UpdateTarget();
        transform.localRotation = Quaternion.Euler(-currentRotation.y-25, currentRotation.x, 0);
    }
}
