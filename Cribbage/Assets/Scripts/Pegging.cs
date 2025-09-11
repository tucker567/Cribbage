using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pegging : MonoBehaviour
{
    public CardDropHandler cardDropHandler; // Reference to the CardDropHandler script

    // Temporary placeholder for the score during pegging
    public int currentScore = 0;
    public TextMeshProUGUI scoreText;
    public void AddToScore(int points)
    {
        currentScore += points;
        Debug.Log("Current Pegging Score: " + currentScore);
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
    
}
