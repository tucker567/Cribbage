using UnityEngine;
using UnityEngine.EventSystems;

public class PegUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PegDefinition peg;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private bool canDrag = true; // Add this field

    public void Setup(PegDefinition pegDefinition, Canvas parentCanvas, bool draggable = true)
    {
        peg = pegDefinition;
        canvas = parentCanvas;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canDrag = draggable; // Set drag state

        GetComponentInChildren<UnityEngine.UI.Image>().sprite = peg.artwork;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{peg.pegName}\nCost: {peg.cost}";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // Prevent drag
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // Prevent drag
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return; // Prevent drag
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        PegInventory pegInventory = FindObjectOfType<PegInventory>();

        if (pegInventory == null || pegInventory.inventoryUIParent == null)
        {
            Debug.LogError("PegInventory or inventoryUIParent is not assigned.");
            rectTransform.anchoredPosition = Vector2.zero;
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(
            pegInventory.inventoryUIParent.GetComponent<RectTransform>(),
            eventData.position,
            canvas.worldCamera))
        {
            transform.SetParent(pegInventory.inventoryUIParent, false);

            Vector2 startPosition = new Vector2(0, 0);
            Vector2 offset = new Vector2(100, 0);
            int index = pegInventory.ownedPegs.Count;
            rectTransform.anchoredPosition = startPosition + (offset * index);

            pegInventory.ownedPegs.Add(peg);

            canDrag = false; // Disable dragging after added to inventory

            Debug.Log($"Peg {peg.pegName} moved to inventory.");
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}