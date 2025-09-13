using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HandUIController : MonoBehaviour
{
    public Transform handArea;
    public GameObject cardUIPrefab;
    public List<GameObject> cardUIObjects = new List<GameObject>();
    private List<GameObject> fixedCardUIObjects = new List<GameObject>(); // Cards not to be shuffled
    public GameObject playArea; // Reference to the PlayArea GameObject

    public Pegging pegging; // Assign in inspector or via script
    public MainDeck MainDeck; // Assign in inspector or via script
    public GameObject deck; // Reference to the Deck GameObject/image
    public CardDropHandler cardDropHandler; // Reference to CardDropHandler script
    public GameObject discardPile; // Assign this in the inspector

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

        // Move any cards left in hand to playArea
        for (int i = cardUIObjects.Count - 1; i >= 0; i--)
        {
            GameObject card = cardUIObjects[i];
            card.transform.SetParent(playArea.transform);
            cardUIObjects.RemoveAt(i);
        }

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

        // Add a random starter card from the deck, spawn it at the deck's position
        GameObject starterCardGO = MainDeck != null ? MainDeck.GetRandomCardNotInList(playedCards) : null;
        if (starterCardGO != null)
        {
            // Set starter card's parent to playArea, but spawn at deck's position
            starterCardGO.transform.SetParent(playArea.transform);
            RectTransform starterRect = starterCardGO.GetComponent<RectTransform>();
            if (deck != null && starterRect != null)
            {
                // Set position to deck's position in playArea's local space
                Vector2 deckLocalPos = playArea.transform.InverseTransformPoint(deck.transform.position);
                starterRect.anchoredPosition = deckLocalPos;
            }
            playedCards.Add(starterCardGO);
        }
        else
        {
            Debug.LogWarning("Starter card could not be generated!");
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
        float downY = -200f; // Use a fixed Y position for all cards
        for (int i = 0; i < playedCards.Count; i++)
        {
            RectTransform cardTransform = playedCards[i].GetComponent<RectTransform>();
            if (cardTransform != null)
            {
                Vector2 targetPosition = new Vector2(startX + i * spacing, downY);
                StartCoroutine(AnimateCardToPosition(cardTransform, targetPosition));
            }
        }

        // After arranging and animating, start coroutine to score after delay
        StartCoroutine(ScoreHandAfterAnimation(playedCards, starterCardGO));
    }

    private IEnumerator ScoreHandAfterAnimation(List<GameObject> playedCards, GameObject starterCardGO)
    {
        float animationDuration = 1.6f;
        yield return new WaitForSeconds(animationDuration);

        List<CardUIController> hand = new List<CardUIController>();
        CardUIController starterCard = null;
        foreach (var go in playedCards)
        {
            var cardUI = go.GetComponent<CardUIController>();
            if (cardUI != null)
            {
                if (go == starterCardGO)
                    starterCard = cardUI;
                else
                    hand.Add(cardUI);
            }
        }
        if (pegging != null && hand.Count == 4 && starterCard != null)
        {
            yield return pegging.StartCoroutine(pegging.ScoreFinalHandWithTextSequential(hand, starterCard));
            Debug.Log("Final hand scored (sequential): " + pegging.currentScore + " points");
        }
        else
        {
            Debug.LogWarning("Scoring skipped: hand.Count=" + hand.Count + ", starterCard=" + (starterCard != null));
        }

        // Call DiscardHandScoring after scoring the first hand
        DiscardHandScoring();
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

    public void DiscardHandScoring()
    {
        List<GameObject> discardedCards = new List<GameObject>();
        for (int i = 0; i < discardPile.transform.childCount; i++)
        {
            Transform child = discardPile.transform.GetChild(i);
            CardUIController cardUI = child.GetComponent<CardUIController>();
            if (cardUI != null && cardUI.isDiscarded)
            {
                discardedCards.Add(child.gameObject);
            }
        }

        if (discardedCards.Count != 2)
        {
            Debug.LogError("Crib hand must have exactly 2 discarded cards.");
            return;
        }

        // Step 2: Draw three new cards from the deck (not already in playArea)
        List<GameObject> newCards = new List<GameObject>();
        List<GameObject> exclude = new List<GameObject>(discardedCards);
        for (int i = 0; i < 3; i++)
        {
            GameObject newCard = MainDeck.GetRandomCardNotInList(exclude);
            if (newCard != null)
            {
                newCards.Add(newCard);
                exclude.Add(newCard); // Prevent duplicates
            }
            else
            {
                Debug.LogError("Not enough cards in the deck to draw three new cards.");
                return;
            }
        }

        // Step 3: Combine the discarded cards and new cards into a new crib hand
        List<GameObject> cribHand = new List<GameObject>();
        cribHand.AddRange(discardedCards);
        cribHand.AddRange(newCards);

        // Step 4: Shuffle the cribHand list
        for (int i = 0; i < cribHand.Count; i++)
        {
            int rnd = Random.Range(i, cribHand.Count);
            GameObject temp = cribHand[rnd];
            cribHand[rnd] = cribHand[i];
            cribHand[i] = temp;
        }

        // Step 5: Arrange the crib hand underneath the played cards (centered, spaced, lower Y)
        float spacing = 100f;
        float startX = -(cribHand.Count - 1) * spacing / 2;
        float downY = -350f; // Lower than played cards
        for (int i = 0; i < cribHand.Count; i++)
        {
            GameObject card = cribHand[i];
            card.transform.SetParent(playArea.transform);
            RectTransform cardTransform = card.GetComponent<RectTransform>();
            if (cardTransform != null)
            {
                Vector2 targetPosition = new Vector2(startX + i * spacing, downY);
                StartCoroutine(AnimateCardToPosition(cardTransform, targetPosition));
            }
        }

        // Step 6: Tally up the points using the existing scoring logic (no starter card for crib)
        List<CardUIController> cardControllers = new List<CardUIController>();
        foreach (GameObject card in cribHand)
        {
            CardUIController cardUI = card.GetComponent<CardUIController>();
            if (cardUI != null)
            {
                cardControllers.Add(cardUI);
            }
        }

        // Step 7: Score after animation
        StartCoroutine(ScoreCribAfterAnimation(cardControllers));
    }

    private IEnumerator ScoreCribAfterAnimation(List<CardUIController> cribCards)
    {
        float animationDuration = 1.6f;
        yield return new WaitForSeconds(animationDuration);

        if (pegging != null && cribCards.Count == 5)
        {
            // Score the crib hand (no starter card)
            yield return pegging.StartCoroutine(pegging.ScoreFinalHandWithTextSequential(cribCards, null));
            Debug.Log("Crib hand scored (sequential): " + pegging.currentScore + " points");
        }
        else
        {
            Debug.LogError("Crib scoring failed: incorrect number of cards or missing Pegging reference.");
        }
    }
}