using UnityEngine;
using System.Collections.Generic;

public class MainDeck : MonoBehaviour
{
    [Header("Main Card Definitions")]
    public List<CardDefinition> CardDefinitions; // Reference to ScriptableObject cards

    // Returns a random card GameObject not in the provided list
    public GameObject GetRandomCardNotInList(List<GameObject> exclude)
    {
        HashSet<string> usedCards = new HashSet<string>();
        foreach (var go in exclude)
        {
            CardUIController ui = go.GetComponent<CardUIController>();
            if (ui != null)
                usedCards.Add(ui.suitNumber + "_" + ui.number);
        }

        List<CardDefinition> available = new List<CardDefinition>();
        foreach (var def in CardDefinitions)
        {
            string key = def.suitNumber + "_" + def.number;
            if (!usedCards.Contains(key))
                available.Add(def);
        }

        if (available.Count == 0) return null;

        CardDefinition chosen = available[Random.Range(0, available.Count)];
        HandUIController handUI = FindObjectOfType<HandUIController>();
        GameObject cardUIPrefab = handUI.cardUIPrefab;
        GameObject starterCard = Instantiate(cardUIPrefab);

        CardUIController cardUI = starterCard.GetComponent<CardUIController>();
        cardUI.SetCardArt(chosen.artwork);
        cardUI.SetCardNumberAndSuit(chosen.suitNumber, chosen.number);

        // Spawn at deck position
        if (handUI.deck != null)
        {
            starterCard.transform.SetParent(handUI.deck.transform.parent, false);
            starterCard.transform.position = handUI.deck.transform.position;
        }

        return starterCard;
    }
}