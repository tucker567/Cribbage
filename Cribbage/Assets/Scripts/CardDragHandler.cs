using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 startPosition;
    public Transform originalParent;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        originalParent = transform.parent;

        // Make the card draggable
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the card's position to follow the mouse
        transform.position = Input.mousePosition;

        // make transparent
        canvasGroup.alpha = 0.8f;

        // rotate slightly based on horizontal movement
        float rotationZ = Mathf.Clamp((Input.mousePosition.x - startPosition.x) / 20f, -15f, 15f);
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore raycast blocking
        canvasGroup.blocksRaycasts = true;
        // Restore full opacity
        canvasGroup.alpha = 1f;

        // Restore rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Check if the card was dropped in a valid drop zone
        if (transform.parent == originalParent)
        {
            // If not, return the card to its original position
            transform.position = startPosition;
        }
    }
}
