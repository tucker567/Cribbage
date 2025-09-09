using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
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

                // Optionally, snap the card to the center of the drop zone
                droppedCard.transform.position = transform.position;

                Debug.Log("Card successfully dropped in a valid drop zone.");
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