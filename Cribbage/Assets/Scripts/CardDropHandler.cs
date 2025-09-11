using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    // find the prefab card canvas group
    public CanvasGroup canvasGroup;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;

        if (droppedCard != null && droppedCard.CompareTag("Card"))
        {
            // Check if this GameObject is a valid drop zone
            if (CompareTag("DropZone"))
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
                    Debug.Log("Card successfully dropped in a valid drop zone: " + droppedCard.name + " (Suit: " + cardUI.suitNumber + ", Number: " + cardUI.number + ")");
                }

                // Tell Pegging that this card has been played

                // Card that has been played can no longer be dragged
                    CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
                if (dragHandler != null)
                {
                    Destroy(dragHandler);
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
}