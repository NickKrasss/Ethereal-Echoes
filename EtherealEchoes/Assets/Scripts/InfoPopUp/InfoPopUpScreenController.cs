using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InfoPopUpScreenController : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Animator animator;

    // Singleton instance
    public static InfoPopUpScreenController Instance { get; private set; }
    private void Awake() => Instance = this;

    // Method to show the pop up with a message with an optional delay
    public void Show(string message, float duration, float delay = 0f, Action action = null)
    {
        // If delay is not zero, start a coroutine to wait and then call Show again
        if (delay != 0f)
        {
            StartCoroutine(waitAndInvokeAction(delay, () =>
            {
                Show(message, duration, 0f); // Call Show again with no delay
            }));
            return;
        }

        // if pop up is already showing, hide it and show the new message
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Show"))
        {
            animator.SetTrigger("HidePopUp");

            // wait for 0.5 seconds before showing the new message
            StartCoroutine(waitAndInvokeAction(0.5f, () =>
            {
                text.text = message;
                animator.SetTrigger("ShowPopUp");

                // wait for the specified duration and then hide the pop up
                StartCoroutine(waitAndInvokeAction(duration, () =>
                {
                    animator.SetTrigger("HidePopUp");

                    // If an action is provided, invoke it after the duration
                    action?.Invoke();
                }));
            }));
        }
        else
        {
            // if pop up is not showing, show it with the new message
            text.text = message;
            animator.SetTrigger("ShowPopUp");

            // wait for 5 seconds and then hide the pop up
            StartCoroutine(waitAndInvokeAction(duration, () =>
            {
                animator.SetTrigger("HidePopUp");

                // If an action is provided, invoke it after the duration
                action?.Invoke();
            }));
        }
    }

    public void ShowMultiple(float duration, float delay = 0f, params string[] texts)
    {
        if (texts.Length == 0)
            return;

        // If delay is not zero, start a coroutine to wait and then call Show again
        if (delay != 0f)
        {
            StartCoroutine(waitAndInvokeAction(delay, () =>
            {
                ShowMultiple(duration, 0f, texts); // Call Show again with no delay
            }));
            return;
        }

        // show the first text and then recursively show the rest
        Show(texts[0], duration, 0f, () => ShowMultiple(duration, 0f, texts.Skip(1).ToArray()));
    }

    // Coroutine to wait for a specified duration and then invoke an action
    IEnumerator waitAndInvokeAction(float duration, Action action)
    {
        yield return new WaitForSeconds(duration);
        action.Invoke();
    }
}