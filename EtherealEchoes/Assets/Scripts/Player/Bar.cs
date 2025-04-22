using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private RectTransform line;
    [SerializeField] private RectTransform backLine;
    [SerializeField] private RawImage barSlot;

    private float barSlotStartScale;
    private float barSlotStartWidth;

    [SerializeField] private float slotPerHP = 10f;

    private Animation anim;

    [SerializeField]
    private float backLineSpeed = 5f;

    [SerializeField] private GameObject gear;
    [SerializeField] private float gearSpeed;
    private float gearSpeedMult = 1;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (gear != null)
        {
            gear.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * gearSpeed * gearSpeedMult));
        }
    }

    public void Shake()
    {
        anim.Play();
    }

    public void SetValue(float value)
    {
        SetLine(value);
        SetBackLine(backLineSpeed * Time.deltaTime);
        gearSpeedMult = 7.5f - value*5;
    }

    public void SetMaxHP(float maxHP)
    {
        float slots = maxHP / slotPerHP;
        barSlot.uvRect = new Rect(0, 0, slots * 2, 1);
    }

    private void SetLine(float x)
    {
        line.localScale = new Vector3(x, 1);
    }

    private void SetBackLine(float speed)
    {
        float current = backLine.localScale.x;
        float target = line.localScale.x;
        backLine.localScale = new Vector3(
            Mathf.Lerp(
                current,
                target, 
                speed * (Mathf.Abs(current-target) + 0.5f)
                ),
            1);
    }
}
