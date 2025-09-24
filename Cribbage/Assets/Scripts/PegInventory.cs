using System.Collections.Generic;
using UnityEngine;

public class PegInventory : MonoBehaviour
{
    public List<PegDefinition> ownedPegs = new List<PegDefinition>();
    public Transform inventoryUIParent; // Parent object for inventory UI
    public GameObject pegUIPrefab; // Prefab for displaying pegs in the inventory

    public void AddPeg(PegDefinition peg)
    {
        if (!ownedPegs.Contains(peg))
        {
            ownedPegs.Add(peg);
            Debug.Log($"Peg {peg.pegName} added to inventory.");

            // Add the peg to the inventory UI
            GameObject pegUI = Instantiate(pegUIPrefab, inventoryUIParent);
            pegUI.GetComponent<PegUI>().Setup(peg, inventoryUIParent.GetComponentInParent<Canvas>());

            Vector2 startPosition = new Vector2(0, 0); // Starting position
            Vector2 offset = new Vector2(100, 0); // Spacing between pegs
            int index = ownedPegs.Count - 1; // Current index
            pegUI.GetComponent<RectTransform>().anchoredPosition = startPosition + (offset * index);
        }
    }
}