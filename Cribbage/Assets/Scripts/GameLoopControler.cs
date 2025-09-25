using UnityEngine;
using System.Collections;
using TMPro;

public class GameLoopControler : MonoBehaviour
{
    [Header("Script References")]
    public DeckController deckController;
    public Pegging pegging;
    public HandUIController handUIController;

    [Header("UI References")]
    public GameObject drawButton;
    public GameObject roundText;

    // Different canvases
    public GameObject ShopScreenCanvas;
    public GameObject GameScreenCanvas;
    public GameObject PegCanvas;

    [Header("Different encounter round num")]
    public int roundsBeforeShop = 2;

    private int playerScore = 0;
    private int roundNumber = 0;
    private int roundsSinceShop = 0; // Track rounds played since last shop

    // Event for shop
    public delegate void ShopEventHandler();
    public event ShopEventHandler OnShopEvent;

    // List to control different options in the game loop, e.g., shop, boss, ecounter etc.
    public enum GameState
    {
        InShop,
        InEncounter,
        InBossFight,
        InRound
    }

    void Start()
    {
        roundNumber = 1;
        Debug.Log("GameLoopControler Start: Setting round to 1 and starting game.");
        StartGame();
    }

    private GameState currentState = GameState.InRound;

    // Call this to start the game and ensure round 1 is the first round
    public void StartGame()
    {
    roundNumber = 1; // Start at round 1
    Debug.Log("Game started. Beginning at round 1.");
    StartNewRound();
    }

    public void StartNewRound()
    {
        Debug.Log("Starting round " + roundNumber);
        if (roundText != null)
        {
            roundText.SetActive(true);
            roundText.GetComponent<TextMeshProUGUI>().text = "Round " + roundNumber;
        }
        currentState = GameState.InRound;

        // Reset deck
        deckController.InitializeDeck();
        deckController.ShuffleDeck();

        // Clear hand UI
        handUIController.cardUIObjects.ForEach(card => Destroy(card));
        handUIController.cardUIObjects.Clear();
        pegging.countText.text = "Count: " + 0;

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
        Debug.Log($"Round {roundNumber} ended. Player score: {playerScore}");
        roundNumber++; // Only increment after a round is completed
        roundsSinceShop++;
        if (roundsSinceShop >= roundsBeforeShop)
        {
            Debug.Log($"Shop event triggered after {roundsSinceShop} rounds.");
            currentState = GameState.InShop;
            Shop();
            roundsSinceShop = 0;
        }
        else
        {
            StartCoroutine(WaitAndStartNextRound());
        }
    }

    private IEnumerator WaitAndStartNextRound()
    {
        yield return new WaitForSeconds(2f); // 2 second delay
        StartNewRound();
    }

    // Example shop event method
    public void Shop()
    {
        // Enable shop canvas and disable game canvas
        if (ShopScreenCanvas != null && GameScreenCanvas != null)
        {
            ShopScreenCanvas.SetActive(true);
            PegCanvas.SetActive(true);
            GameScreenCanvas.SetActive(false);
        }
        Debug.Log("Shop is now open!");
    }

    // This method should be called by the shop button when the player is done shopping
    public void ExitShopAndContinue()
    {
        if (ShopScreenCanvas != null && GameScreenCanvas != null)
        {
            ShopScreenCanvas.SetActive(false);
            PegCanvas.SetActive(false);
            GameScreenCanvas.SetActive(true);
        }
    Debug.Log("Exiting shop, continuing to next round.");
    currentState = GameState.InRound;
    StartNewRound();
    }
}
