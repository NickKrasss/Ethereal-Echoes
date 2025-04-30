using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TMP_Text worldNameText;
    [SerializeField] TMP_Text timerText;

    public static UIController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        worldNameText.color = new Color(worldNameText.color.r, worldNameText.color.g, worldNameText.color.b, 0);

        // Check if the tutorial has been seen before
        if (!PlayerPrefs.HasKey("TutorialSeen"))
        {
            // Show the tutorial pop-up
            InfoPopUpScreenController.Instance.ShowMultiple(5f, 2f, "Movement - <color=grey>WASD</color>\r\nAttack - <color=grey>Left Mouse</color>\r\nIncrese level - <color=grey>Hold R</color>", "Defeat all the enemies!\nTry not to die..\n<color=red>Good luck :)</color>");

            // Set the PlayerPrefs key to indicate that the tutorial has been seen
            PlayerPrefs.SetInt("TutorialSeen", 1);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(G.Instance.currentTime);
        timerText.text = time.ToString(@"mm\:ss");
    }

    public void ShowWorldName(string worldName)
    {
        StopAllCoroutines();
        StartCoroutine(fadeInAndOut(3f, 2f, 5f, worldName));
    }
    IEnumerator fadeInAndOut(float duration, float delay, float secondDelay, string message)
    {
        yield return new WaitForSeconds(delay);

        worldNameText.color = new Color(worldNameText.color.r, worldNameText.color.g, worldNameText.color.b, 0);
        worldNameText.text = message;
        Color originalColor = worldNameText.color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            worldNameText.color = Color.Lerp(originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 1), elapsedTime / duration);
            yield return null;
        }

        worldNameText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

        yield return new WaitForSeconds(secondDelay);

        originalColor = worldNameText.color;
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            worldNameText.color = Color.Lerp(originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 0), elapsedTime / duration);
            yield return null;
        }

        worldNameText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
