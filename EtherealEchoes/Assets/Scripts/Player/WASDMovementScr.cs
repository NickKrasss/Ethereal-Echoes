using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SmoothMoveScr))]
[RequireComponent(typeof(Stats))]
public class WASDMovementScr : MonoBehaviour
{
    private Stats stats;

    public Vector2 wasdVector;

    // Клавиши для передвижения
    private Dictionary<string, KeyCode> movementKeys = new Dictionary<string, KeyCode> {
        { "up",    KeyCode.W },
        { "left",  KeyCode.A },
        { "down",  KeyCode.S },
        { "right", KeyCode.D }
    };

    private SmoothMoveScr smoothScr;


    // Вычисляет targetMoveVector и обновляет его в SmoothMoveScr
    private void UpdateTargetVector()
    {
        wasdVector = Vector2.zero;
        if (Input.GetKey(movementKeys["up"]))
            wasdVector += new Vector2(0, 1);  // Up

        if (Input.GetKey(movementKeys["left"]))
            wasdVector += new Vector2(-1, 0); // Left

        if (Input.GetKey(movementKeys["down"]))
            wasdVector += new Vector2(0, -1); // Down

        if (Input.GetKey(movementKeys["right"]))
            wasdVector += new Vector2(1, 0);  // Right

        wasdVector = wasdVector.normalized * stats.MoveSpeed;              // В любую сторону длина вектора будет равна moveSpeed

        smoothScr.targetMoveVector = wasdVector;
    }

    // Обновляет бинды из настроек
    private void UpdateKeyBinds()
    {
        if (PlayerPrefs.HasKey("UpKeyCode")) movementKeys["up"] = (KeyCode) PlayerPrefs.GetInt("UpKeyCode");
        if (PlayerPrefs.HasKey("LeftKeyCode")) movementKeys["left"] = (KeyCode)PlayerPrefs.GetInt("LeftKeyCode");
        if (PlayerPrefs.HasKey("DownKeyCode")) movementKeys["down"] = (KeyCode)PlayerPrefs.GetInt("DownKeyCode");
        if (PlayerPrefs.HasKey("RightKeyCode")) movementKeys["right"] = (KeyCode)PlayerPrefs.GetInt("RightKeyCode");
    }

    private void Start()
    {
        smoothScr = GetComponent<SmoothMoveScr>(); // Находит компонент SmoothMoveScr
        stats = GetComponent<Stats>();
    }

    private void Update() 
    {
        UpdateTargetVector();
    }
}
