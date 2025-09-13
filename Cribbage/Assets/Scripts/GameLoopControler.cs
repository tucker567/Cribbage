using UnityEngine;
using System.Collections;

public class GameLoopControler : MonoBehaviour
{
    public DeckController deckController;
    public Pegging pegging;
    public HandUIController handUIController;
    public GameObject drawButton; // Assign in inspector

    private int playerScore = 0;
    private int roundNumber = 0;

    public void StartNewRound()
    {
        roundNumber++;
        Debug.Log("Starting round " + roundNumber);

        // Reset deck
        deckController.InitializeDeck();
        deckController.ShuffleDeck();

        // Clear hand UI
        handUIController.cardUIObjects.ForEach(card => Destroy(card));
        handUIController.cardUIObjects.Clear();

        Debug.Log("Deck shuffled and hand cleared.");

        // Reset pegging state but keep playerScore
        pegging.runningTotal = 0;
        pegging.currentScore = playerScore;
        pegging.UpdateScoreDisplay();
        Debug.Log("Pegging reset. Current score: " + pegging.currentScore);

        // Reset discard count for new round
        CardDropHandler.discardedCardCount = 0;

        // Hide done button and show count text at start of round
        if (handUIController.cardDropHandler != null)
        {
            if (handUIController.cardDropHandler.doneButton != null)
                handUIController.cardDropHandler.doneButton.SetActive(false);
            if (handUIController.cardDropHandler.countText != null)
                handUIController.cardDropHandler.countText.SetActive(true);
        }

        // Enable draw button (assuming you have a reference to it)
        if (drawButton != null)
            drawButton.SetActive(true);

        // Remove only CardUI(Clone) children from play area
        if (handUIController.playArea != null)
        {
            foreach (Transform child in handUIController.playArea.transform)
            {
                if (child.name == "CardUI(Clone)")
                {
                    Destroy(child.gameObject);
                }
            }
        }

        Debug.Log("New round setup complete. Waiting for player to draw cards.");
    }

    public void EndRound()
    {
        playerScore = pegging.currentScore;
        Debug.Log("Round ended. Player score: " + playerScore);

        // Wait for player input or a delay before starting next round
        StartCoroutine(WaitAndStartNextRound());
    }

    private IEnumerator WaitAndStartNextRound()
    {
        yield return new WaitForSeconds(2f); // 2 second delay
        StartNewRound();
    }
}
