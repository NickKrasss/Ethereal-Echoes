using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCardsController : MonoBehaviour
{
    // Singleton
    public static PowerUpCardsController Instance { get; private set; }
    private void Awake() => Instance = this;

    // list of power up cards
    [SerializeField] List<GameObject> powerUpCards_common, powerUpCards_rare, powerUpCards_epic, powerUpCards_legendary;
    [Space(10)]

    // UI elements
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] List<Transform> powerUpCardPlaceholders;
    [SerializeField] Animator animator;

    // list of instantiated power up cards
    List<PowerUpCard> cards = new List<PowerUpCard>();
    List<string> listOfCurrentCards = new List<string>();
    List<PowerUpCard> cardsUsedInGame = new List<PowerUpCard>();
    int generatedCardsCount = 0;
    Coroutine delayedActionCoroutine;

    private void Initialize(Vector3 probabilityRanges)
    {

        // clear the list of cards
        cards = new List<PowerUpCard>();

        for (int i = 0; i < 3; i++)
        {
            // generate a random 
            PowerUpCard card = null;
            

            while (card == null)
            {
                int probability = UnityEngine.Random.Range(0, 100);

                // instantiate a card based on the probability
                if (probability < probabilityRanges.x)
                    card = InstantiatePowerUpCard(powerUpCards_common, powerUpCardPlaceholders[i]);
                else if (probability < probabilityRanges.y && probability >= probabilityRanges.x)
                    card = InstantiatePowerUpCard(powerUpCards_rare, powerUpCardPlaceholders[i]);
                else if (probability < probabilityRanges.z && probability >= probabilityRanges.y)
                    card = InstantiatePowerUpCard(powerUpCards_epic, powerUpCardPlaceholders[i]);
                else
                    card = InstantiatePowerUpCard(powerUpCards_legendary, powerUpCardPlaceholders[i]);



                if (!listOfCurrentCards.Contains(card.cardName))
                {
                    listOfCurrentCards.Add(card.cardName);
                    cards.Add(card);
                    cardsUsedInGame.Add(card);
                    break;
                }
                else
                {
                    card = null;
                }
            }
        }

        // set up mouse hover and click actions for each card
        // TODO: refactor this bruh
        cards[0].actionOnMouseHover += () => ChangeDescription(cards[0].cardDescription);
        cards[1].actionOnMouseHover += () => ChangeDescription(cards[1].cardDescription);
        cards[2].actionOnMouseHover += () => ChangeDescription(cards[2].cardDescription);

        cards[0].actionOnClick += () => Close();
        cards[1].actionOnClick += () => Close();
        cards[2].actionOnClick += () => Close();

        // trigger the opening animation
        animator.SetTrigger("OpenScreen");
        listOfCurrentCards = new List<string>();
    }

    void activityCard()
    {

    }

    void Close()
    {
        // stop any existing coroutine
        if (delayedActionCoroutine != null)
            StopCoroutine(delayedActionCoroutine);

        // trigger the closing animation
        animator.SetTrigger("CloseScreen");

        // remove mouse hover and click actions for each card
        // TODO: refactor this bruh
        cards[0].actionOnMouseHover -= () => ChangeDescription(cards[0].cardDescription);
        cards[1].actionOnMouseHover -= () => ChangeDescription(cards[1].cardDescription);
        cards[2].actionOnMouseHover -= () => ChangeDescription(cards[2].cardDescription);

        cards[0].actionOnClick -= () => Close();
        cards[1].actionOnClick -= () => Close();
        cards[2].actionOnClick -= () => Close();

        // start a coroutine to destroy the cards after a delay
        delayedActionCoroutine = StartCoroutine(waitAndPerformAction(() =>
        {
            Destroy(cards[0].gameObject);
            Destroy(cards[1].gameObject);
            Destroy(cards[2].gameObject);

            cards.Clear();
        }, 
        2f));
    }

    // Coroutine to wait for a specified time and then perform an action
    IEnumerator waitAndPerformAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    // Method to instantiate a power up card from a pool
    PowerUpCard InstantiatePowerUpCard(List<GameObject> pool, Transform parent)
    {
        PowerUpCard card = Instantiate(pool[UnityEngine.Random.Range(0, pool.Count)], parent.position, Quaternion.identity, parent).GetComponentInChildren<PowerUpCard>();
        return card;
    }

    // Method to change the description of the card
    void ChangeDescription(string text) =>
        cardDescription.text = text;

    private void Start()
    {
        Vector3 def = new (0, 0, 100);
        Initialize(def);
    }
}
