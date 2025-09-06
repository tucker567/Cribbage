// 2025-09-05 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public MainDeck mainDeck; // Reference to the MainDeck script
    private List<CardDefinition> currentDeck = new List<CardDefinition>();
    private List<CardDefinition> playerHand = new List<CardDefinition>();

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    // Initialize the deck with all cards from the MainDeck
    private void InitializeDeck()
    {
        currentDeck = new List<CardDefinition>(mainDeck.CardDefinitions);
    }

    // Shuffle the deck
    private void ShuffleDeck()
    {
        for (int i = 0; i < currentDeck.Count; i++)
        {
            int randomIndex = Random.Range(0, currentDeck.Count);
            CardDefinition temp = currentDeck[i];
            currentDeck[i] = currentDeck[randomIndex];
            currentDeck[randomIndex] = temp;
        }
    }

    // Draw a single card randomly
    public CardDefinition DrawSingleCard()
    {
        if (currentDeck.Count > 0)
        {
            CardDefinition drawnCard = currentDeck[0]; // Take the top card
            currentDeck.RemoveAt(0); // Remove it from the deck
            playerHand.Add(drawnCard); // Add it to the player's hand
            return drawnCard;
        }
        else
        {
            Debug.Log("No more cards in the deck!");
            return null;
        }
    }
}