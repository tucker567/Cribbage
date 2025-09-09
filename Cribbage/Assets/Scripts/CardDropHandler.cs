using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;

        if (droppedCard != null)
        {
            RectTransform cardTransform = droppedCard.GetComponent<RectTransform>();
            cardTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            Debug.Log($"Card {droppedCard.name} dropped in {gameObject.name}");
        }
    }
}
