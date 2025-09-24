using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public PegInventory playerInventory;
    public int playerCurrency = 100; // Example starting currency
    public List<PegDefinition> pegPool; // Pool of all available pegs
    public Transform pegDisplayArea; // Parent object for displaying pegs
    public GameObject pegUIPrefab; // Prefab for displaying pegs in the shop
    public Canvas shopCanvas; // Reference to the Canvas containing the shop UI

    private List<PegDefinition> currentShopPegs = new List<PegDefinition>();

    public void PopulateShop(int numberOfPegs)
    {
        ClearShopDisplay();

        for (int i = 0; i < numberOfPegs; i++)
        {
            if (pegPool.Count > 0)
            {
                int randomIndex = Random.Range(0, pegPool.Count);
                PegDefinition randomPeg = pegPool[randomIndex];
                currentShopPegs.Add(randomPeg);

                // Instantiate UI for the peg
                GameObject pegUI = Instantiate(pegUIPrefab, pegDisplayArea);
                pegUI.GetComponent<PegUI>().Setup(randomPeg, shopCanvas); // Pass the Canvas reference here
            }
        }
    }

    void Start()
    {
        PopulateShop(1); // Example: populate shop with 1 peg at start
    }

    public void BuyPeg(PegDefinition peg)
    {
        if (playerCurrency >= peg.cost)
        {
            playerCurrency -= peg.cost;
            playerInventory.AddPeg(peg);
            Debug.Log($"Bought {peg.pegName} for {peg.cost} currency. Remaining: {playerCurrency}");
        }
        else
        {
            Debug.Log("Not enough currency to buy this peg.");
        }
    }

    private void ClearShopDisplay()
    {
        foreach (Transform child in pegDisplayArea)
        {
            Destroy(child.gameObject);
        }
        currentShopPegs.Clear();
    }
}