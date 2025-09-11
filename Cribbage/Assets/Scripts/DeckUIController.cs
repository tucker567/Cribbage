using UnityEngine;

public class DeckUIController : MonoBehaviour
{
    public GameObject drawButton; // Reference to the button that triggers drawing cards
    public DeckController deckController; // Reference to the DeckController
    public HandUIController handUIController; // Reference to the HandUIController
    public int numberOfCardsToDraw = 5; // Public variable to specify how many cards to draw

    public void DrawCards()
    {
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            CardDefinition drawnCard = deckController.DrawSingleCard(); // Draw a card from the deck
            if (drawnCard != null)
            {
                handUIController.AddCardToHand(drawnCard.artwork, drawnCard.suitNumber, drawnCard.number); // Add the card to the UI hand
            }
            else
            {
                Debug.Log("No more cards in the deck!");
                break; // Stop drawing if the deck is empty
            }
        }
    }
}