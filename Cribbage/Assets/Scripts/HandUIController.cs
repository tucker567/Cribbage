using System.Collections.Generic;
using UnityEngine;

public class HandUIController : MonoBehaviour
{
    public Transform handArea;
    public GameObject cardUIPrefab;
    public List<GameObject> cardUIObjects = new List<GameObject>();
    private List<GameObject> fixedCardUIObjects = new List<GameObject>(); // Cards not to be shuffled

    public void AddCardToHand(Sprite cardArtwork, string suitNumber, string cardNumber)
    {
        GameObject newCard = Instantiate(cardUIPrefab, handArea);
        CardUIController cardUI = newCard.GetComponent<CardUIController>();
        cardUI.SetCardArt(cardArtwork);
        cardUI.SetCardNumberAndSuit(suitNumber, cardNumber);
        cardUIObjects.Add(newCard);
        ArrangeCardsInHand();
    }

    // Arrange cards in hand with spacing, excluding cards that have been played or are fixed
    public void ArrangeCardsInHand()
    {
        float spacing = 100f;
        float startX = -(cardUIObjects.Count - 1) * spacing / 2;

        int arrangeIndex = 0;
        for (int i = 0; i < cardUIObjects.Count; i++)
        {
            GameObject card = cardUIObjects[i];
            if (fixedCardUIObjects.Contains(card))
                continue; // Skip fixed cards

            RectTransform cardTransform = card.GetComponent<RectTransform>();
            cardTransform.anchoredPosition = new Vector2(startX + arrangeIndex * spacing, 0);
            arrangeIndex++;
        }
    }

    // Add card to hand but don't shuffle/rearrange it
    public void AddExistingCardToHand(GameObject card)
    {
        if (!cardUIObjects.Contains(card))
        {
            cardUIObjects.Add(card);
            fixedCardUIObjects.Add(card); // Mark as fixed
            // Optionally, set its position manually here if needed
        }
    }

    public void RemoveCardFromHand(GameObject card)
    {
        cardUIObjects.Remove(card);
        fixedCardUIObjects.Remove(card);
        ArrangeCardsInHand();
    }
}