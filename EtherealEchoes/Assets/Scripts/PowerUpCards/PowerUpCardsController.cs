using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCardsController : MonoBehaviour
{
    public static PowerUpCardsController Instance { get; private set; }
    private void Awake() => Instance = this;

    [SerializeField] List<GameObject> powerUpCards_common, powerUpCards_rare, powerUpCards_legendary;
    [Space(10)]
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] List<Transform> powerUpCardPlaceholders;
    
    int generatedCardsCount = 0;

    private void Start()
    {
        Vector2 probabilityRanges = new Vector2(50, 80); // Example ranges for common and rare cards
        Initialize(probabilityRanges);
    }

    private void Initialize(Vector2 probabilityRanges)
    {
        List<PowerUpCard> cards = new List<PowerUpCard>();

        for (int i = 0; i < 3; i++)
        {
            int probability = Random.Range(0, 100);
            PowerUpCard card;

            if (probability < probabilityRanges.x)
                card = InstantiatePowerUpCard(powerUpCards_common, powerUpCardPlaceholders[i]);
            else if (probability < probabilityRanges.y && probability >= probabilityRanges.x)
                card = InstantiatePowerUpCard(powerUpCards_rare, powerUpCardPlaceholders[i]);
            else
                card = InstantiatePowerUpCard(powerUpCards_legendary, powerUpCardPlaceholders[i]);

            cards.Add(card);
        }

        cards[0].actionOnMouseHover += () => ChangeDescription(cards[0].cardDescription);
        cards[1].actionOnMouseHover += () => ChangeDescription(cards[1].cardDescription);
        cards[2].actionOnMouseHover += () => ChangeDescription(cards[2].cardDescription);
    }

    PowerUpCard InstantiatePowerUpCard(List<GameObject> pool, Transform parent)
    {
        PowerUpCard card = Instantiate(pool[Random.Range(0, pool.Count)], parent.position, Quaternion.identity, parent).GetComponentInChildren<PowerUpCard>();
        return card;
    }

    void ChangeDescription(string text) =>
        cardDescription.text = text;
}
