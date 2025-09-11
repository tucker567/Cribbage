using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    // find the prefab card canvas group
    public CanvasGroup canvasGroup;
    public static int discardedCardCount = 0; // Track globally
    public Pegging pegging; // Reference to the Pegging script
    public HandUIController handUIController; // Reference to HandUIController
    public GameObject doneButton; // Reference to button to be able to make it visible when two cards have been discarded

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;

        if (droppedCard != null && droppedCard.CompareTag("Card"))
        {
            // Discard pile logic
            if (CompareTag("DiscardPile"))
            {
                if (discardedCardCount < 2)
                {
                    // Set the card's parent to this drop zone
                    droppedCard.transform.SetParent(transform);

                    // Optionally, snap the card to the center of the drop zone and down the y-axis a bit
                    droppedCard.transform.position = new Vector3(transform.position.x, transform.position.y - 80f, transform.position.z);

                    // Get the CanvasGroup component from the card prefab
                    canvasGroup = droppedCard.GetComponent<CanvasGroup>();

                    canvasGroup.alpha = 1f; // Make the drop zone fully visible

                    // Make the cards rotation smaller but not completely flat, between 5 and -5 degrees
                    droppedCard.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));

                    // show debug message of successful drop and card number and suitnumber of card dropped
                    CardUIController cardUI = droppedCard.GetComponent<CardUIController>();
                    if (cardUI != null)
                    {
                        Debug.Log("Card discarded: " + droppedCard.name + " (Suit: " + cardUI.suitNumber + ", Number: " + cardUI.number + ")");
                    }

                    // Tell Pegging that this card has been played

                    // Card that has been played can no longer be dragged
                    CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
                    if (dragHandler != null)
                    {
                        Destroy(dragHandler);
                    }
                    // Remove the card from the hand UI controller's lists
                    handUIController.RemoveCardFromHand(droppedCard);

                    discardedCardCount++;
                    // No need to call ArrangeCardsInHand here
                }
                else
                {
                    Debug.Log("Discard pile already has two cards. Cannot discard more.");
                    // Return the card to its original parent
                    CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
                    droppedCard.transform.SetParent(dragHandler.originalParent);
                    droppedCard.transform.position = dragHandler.startPosition;
                }
            }
            // Play pile logic
            else if (CompareTag("PlayPile"))
            {
                if (discardedCardCount >= 2)
                {
                    // Get needed components before we potentially move or revert
                    CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
                    CardUIController cardUI = droppedCard.GetComponent<CardUIController>();

                    // Score and validate the play (prevent exceeding 31)
                    if (pegging != null && cardUI != null)
                    {
                        if (!pegging.TryPlayCard(cardUI, out int points))
                        {
                            Debug.Log("Illegal play (would exceed 31). Returning card to hand.");
                            // Return the card to its original parent
                            droppedCard.transform.SetParent(dragHandler.originalParent);
                            droppedCard.transform.position = dragHandler.startPosition;
                            return;
                        }
                        else
                        {
                            Debug.Log($"Card played: {droppedCard.name} scored {points} pegging point(s).");
                        }
                        doneButton.SetActive(true); // Show the done button when a card is played

                    }

                    // Set the card's parent to this drop zone
                    droppedCard.transform.SetParent(transform);

                    // Optionally, snap the card to the center of the drop zone and down the y-axis a bit
                    droppedCard.transform.position = new Vector3(transform.position.x, transform.position.y - 80f, transform.position.z);

                    // Get the CanvasGroup component from the card prefab
                    canvasGroup = droppedCard.GetComponent<CanvasGroup>();

                    canvasGroup.alpha = 1f; // Make the drop zone fully visible

                    // Make the cards rotation smaller but not completely flat, between 5 and -5 degrees
                    droppedCard.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));

                    // show debug message of successful drop and card number and suitnumber of card dropped
                    CardUIController cardUIAfter = droppedCard.GetComponent<CardUIController>();
                    if (cardUIAfter != null)
                    {
                        Debug.Log("Card played: " + droppedCard.name + " (Suit: " + cardUIAfter.suitNumber + ", Number: " + cardUIAfter.number + ")");
                    }

                    // Card that has been played can no longer be dragged
                    if (dragHandler != null)
                    {
                        Destroy(dragHandler);
                    }
                    // Remove the card from the hand UI controller's lists
                    handUIController.RemoveCardFromHand(droppedCard);

                    if (handUIController.cardUIObjects.Count == 0)
                    {
                        AllCardsPlayed();
                    }
                }
                else
                {
                    Debug.Log("You must discard two cards before playing.");
                    // Return the card to its original parent
                    CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
                    droppedCard.transform.SetParent(dragHandler.originalParent);
                    droppedCard.transform.position = dragHandler.startPosition;
                }
            }
            else
            {
                Debug.Log("Invalid drop zone. Returning card to hand.");
                // Return the card to its original parent
                CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
                droppedCard.transform.SetParent(dragHandler.originalParent);
                droppedCard.transform.position = dragHandler.startPosition;
            }
        }
    }

    void Start()
    {
        doneButton.SetActive(false); // Hide the done button at the start
    }

    public void AllCardsPlayed()// when all cards have been played, move cards in play area down arrange them, then tally points
    {
        handUIController.ShufflePlayedCards();
    }
}