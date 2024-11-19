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
        Vector2 mousePos = new Vector2(Input.mousePosition.x - Camera.main.pixelWidth/2, Input.mousePosition.y - Camera.main.pixelHeight / 2);
        targetRotation = new Vector2(mousePos.x * mouseForce / Camera.main.pixelWidth, mousePos.y * mouseForce / Camera.main.pixelHeight);
    }

    private void Update()
    {
        UpdateCurrent();
        UpdateTarget();
        transform.localRotation = Quaternion.Euler(-currentRotation.y, currentRotation.x, 0);
    }
}
