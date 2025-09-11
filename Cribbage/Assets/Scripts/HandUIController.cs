using System.Collections.Generic;
using UnityEngine;

public class HandUIController : MonoBehaviour
{
    public Transform handArea;
    public GameObject cardUIPrefab;
    public List<GameObject> cardUIObjects = new List<GameObject>();
    private List<GameObject> fixedCardUIObjects = new List<GameObject>(); // Cards not to be shuffled
    public GameObject playArea; // Reference to the PlayArea GameObject

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

    public void ShufflePlayedCards()
    {
        Debug.Log("Shuffling played cards...");

        // Collect all children of playArea named "CardUI(Clone)"
        List<GameObject> playedCards = new List<GameObject>();
        for (int i = 0; i < playArea.transform.childCount; i++)
        {
            Transform child = playArea.transform.GetChild(i);
            if (child.gameObject.name == "CardUI(Clone)")
            {
                playedCards.Add(child.gameObject);
            }
        }

        // Shuffle the playedCards list
        for (int i = 0; i < playedCards.Count; i++)
        {
            int rnd = Random.Range(i, playedCards.Count);
            GameObject temp = playedCards[rnd];
            playedCards[rnd] = playedCards[i];
            playedCards[i] = temp;
        }

        // Arrange shuffled cards in playArea with spacing, animating them smoothly
        float spacing = 100f;
        float startX = -(playedCards.Count - 1) * spacing / 2;
        float downY = playedCards.Count > 0 ? playedCards[0].GetComponent<RectTransform>().anchoredPosition.y - 80f : 0f;
        for (int i = 0; i < playedCards.Count; i++)
        {
            RectTransform cardTransform = playedCards[i].GetComponent<RectTransform>();
            Vector2 targetPosition = new Vector2(startX + i * spacing, downY);
            StartCoroutine(AnimateCardToPosition(cardTransform, targetPosition));
        }
    }

    // Coroutine to animate card moving down then out to its target position
    private System.Collections.IEnumerator AnimateCardToPosition(RectTransform cardTransform, Vector2 targetPosition)
    {
        Vector2 startPosition = cardTransform.anchoredPosition;
        Vector2 downPosition = new Vector2(startPosition.x, startPosition.y - 80f);
        float durationDown = 0.6f;
        float durationOut = 1.0f;
        float elapsed = 0f;

        // Move down and set rotation back upright
        while (elapsed < durationDown)
        {
            cardTransform.anchoredPosition = Vector2.Lerp(startPosition, downPosition, elapsed / durationDown);
            cardTransform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 5), Quaternion.Euler(0, 0, 0), elapsed / durationDown);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cardTransform.anchoredPosition = downPosition;

        // Move out to target
        elapsed = 0f;
        Vector2 outStart = downPosition;
        while (elapsed < durationOut)
        {
            cardTransform.anchoredPosition = Vector2.Lerp(outStart, targetPosition, elapsed / durationOut);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cardTransform.anchoredPosition = targetPosition;
    }
    
}