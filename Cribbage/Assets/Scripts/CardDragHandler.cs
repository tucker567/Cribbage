using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store the original position in case the card needs to return
        originalPosition = rectTransform.anchoredPosition;

        // Make the card semi-transparent and ignore raycasts while dragging
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the card with the cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset the card's transparency and allow raycasts again
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // code does not work as intended, always returns to original position
        // Intended: Snap back to the original position if not dropped in a valid area
        if (!eventData.pointerEnter || !eventData.pointerEnter.CompareTag("DropZone"))
        {
            rectTransform.anchoredPosition = originalPosition;
            Debug.Log($"Card {gameObject.name} returned to original position");
        }
    }
}