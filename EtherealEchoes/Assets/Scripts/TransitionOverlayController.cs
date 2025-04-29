using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionOverlayController : MonoBehaviour
{
    public static TransitionOverlayController Instance { get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] Image background;
    [SerializeField] TMP_Text text;

    private void Start()
    {
        FadeOut(2f, 1f);
    }

    public void FadeIn(float duration, float delay = 0f)
    {
        StartCoroutine(fadeAction(new Color(0, 0, 0, 1), duration, delay));
    }

    public void FadeOut(float duration, float delay = 0f)
    {
        StartCoroutine(fadeAction(new Color(0, 0, 0, 0), duration, delay));
    }

    IEnumerator fadeAction(Color desiredColor, float time, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        Color startColor = background.color;
        Color startTextColor = text.color;

        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.unscaledDeltaTime;
            text.color = Color.Lerp(startTextColor, new Color(1, 1, 1, desiredColor.a - 1), elapsedTime / time);
            background.color = Color.Lerp(startColor, desiredColor, elapsedTime / time);
            yield return null;
        }
        text.color = new Color(1, 1, 1, desiredColor.a - 1);
        background.color = desiredColor;
    }
}
