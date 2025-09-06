using System.Collections.Generic;
using UnityEngine;

public class HandUIController : MonoBehaviour
{
    public Transform handArea;
    public GameObject cardUIPrefab;
    private List<GameObject> cardUIObjects = new List<GameObject>();

    public void AddCardToHand(Sprite cardArtwork)
    {
        GameObject newCard = Instantiate(cardUIPrefab, handArea);
        CardUIController cardUI = newCard.GetComponent<CardUIController>();
        cardUI.SetCard(cardArtwork);
        cardUIObjects.Add(newCard);
        ArrangeCardsInHand();
    }

    private void ArrangeCardsInHand()
    {
        float spacing = 100f;
        float startX = -(cardUIObjects.Count - 1) * spacing / 2;

        for (int i = 0; i < cardUIObjects.Count; i++)
        {
            RectTransform cardTransform = cardUIObjects[i].GetComponent<RectTransform>();
            cardTransform.anchoredPosition = new Vector2(startX + i * spacing, 0);
        }
    }
}