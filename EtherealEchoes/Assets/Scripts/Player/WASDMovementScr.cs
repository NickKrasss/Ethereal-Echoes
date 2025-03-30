using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(SmoothMoveScr))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(EnergySpender))]
public class WASDMovementScr : MonoBehaviour
{
    private Stats stats;

    private EnergySpender energySpender;

    public Vector2 wasdVector;

    [SerializeField] private float runEnergyCost = 2;
    [SerializeField] private float runMult = 1.4f;

    [SerializeField] private AudioClip[] steps;
    [SerializeField] private float stepVolume;

    // Клавиши для передвижения
    private Dictionary<string, KeyCode> movementKeys = new Dictionary<string, KeyCode> {
        { "up",    KeyCode.W },
        { "left",  KeyCode.A },
        { "down",  KeyCode.S },
        { "right", KeyCode.D },
        { "run", KeyCode.LeftShift }
    };

    private SmoothMoveScr smoothScr;

    public float speed;


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


        if (Input.GetKey(movementKeys["run"]) && wasdVector != Vector2.zero && energySpender.SpendEnergy(Time.deltaTime * runEnergyCost))
        {
                speed = stats.MoveSpeed * runMult;
        }
        else
        {
            speed = stats.MoveSpeed;
        }
        
        wasdVector = wasdVector.normalized * speed;              // В любую сторону длина вектора будет равна moveSpeed

        smoothScr.targetMoveVector = wasdVector;
    }

    public void Step()
    {
        if (AudioManager.Instance)
            AudioManager.Instance.PlayAudio(steps[UnityEngine.Random.Range(0, steps.Length)], SoundType.SFX, stepVolume);
    }

    // Обновляет бинды из настроек
    private void UpdateKeyBinds()
    {
        if (PlayerPrefs.HasKey("UpKeyCode")) movementKeys["up"] = (KeyCode) PlayerPrefs.GetInt("UpKeyCode");
        if (PlayerPrefs.HasKey("LeftKeyCode")) movementKeys["left"] = (KeyCode)PlayerPrefs.GetInt("LeftKeyCode");
        if (PlayerPrefs.HasKey("DownKeyCode")) movementKeys["down"] = (KeyCode)PlayerPrefs.GetInt("DownKeyCode");
        if (PlayerPrefs.HasKey("RightKeyCode")) movementKeys["right"] = (KeyCode)PlayerPrefs.GetInt("RightKeyCode");
        if (PlayerPrefs.HasKey("RunKeyCode")) movementKeys["run"] = (KeyCode)PlayerPrefs.GetInt("RunKeyCode");
    }

    private void Start()
    {
        smoothScr = GetComponent<SmoothMoveScr>(); // Находит компонент SmoothMoveScr
        stats = GetComponent<Stats>();
        energySpender = GetComponent<EnergySpender>();
    }

    private void Update() 
    {
        UpdateTargetVector();
    }
}
