using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private void Start()
    {
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
}
