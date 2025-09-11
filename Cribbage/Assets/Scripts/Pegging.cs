using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pegging : MonoBehaviour
{
    public CardDropHandler cardDropHandler; // Reference to the CardDropHandler script

    // Pegging total since last reset (0..31)
    public int runningTotal = 0;

    // Temporary placeholder for the score during pegging
    public int currentScore = 0;

    // UI
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countText;

    // Last ranks played in the current sequence (since last reset/Go/31)
    private readonly List<int> recentRanks = new List<int>();

    // Try to play a card. Returns false if the play would exceed 31 (card must be returned).
    public bool TryPlayCard(CardUIController cardUI, out int pointsAwarded)
    {
        pointsAwarded = 0;
        if (cardUI == null)
        {
            Debug.LogWarning("Pegging.TryPlayCard: CardUIController missing on played card.");
            return false;
        }

        int rank = GetRank(cardUI);              // 1..13 expected (A..K)
        int val = GetPeggingValue(rank);         // A=1, 2..10=pip, J/Q/K=10

        // Illegal play (cannot exceed 31)
        if (runningTotal + val > 31)
        {
            Debug.Log("Illegal play: would exceed 31.");
            return false;
        }

        // Update running total
        runningTotal += val;

        // Score 15 or 31
        if (runningTotal == 15) pointsAwarded += 2;
        if (runningTotal == 31) pointsAwarded += 2;

        // Pair/Three/Four of a kind: count consecutive same ranks from end (before adding current)
        int pairStreak = 1; // include current
        for (int i = recentRanks.Count - 1; i >= 0; i--)
        {
            if (recentRanks[i] == rank) pairStreak++;
            else break;
        }
        if (pairStreak == 2) pointsAwarded += 2;      // pair
        else if (pairStreak == 3) pointsAwarded += 6; // three-of-a-kind
        else if (pairStreak == 4) pointsAwarded += 12;// four-of-a-kind

        // Add the current rank to the recent list for run detection
        recentRanks.Add(rank);

        // Runs: longest run in the tail segment with all distinct ranks
        int runPoints = ComputeRunPointsFromTail(recentRanks);
        pointsAwarded += runPoints;

        // Apply score
        AddToScore(pointsAwarded);

        // Auto reset the sequence when reaching exactly 31
        if (runningTotal == 31)
        {
            ResetSequence(); // new sequence starts after 31 (no extra last-card point here)
        }

        UpdateCountDisplay();
        return true;
    }

    // Call this when a sequence ends below 31 (e.g., after "go") and you were the last to play.
    public void AwardLastCardPointAndReset()
    {
        AddToScore(1);
        ResetSequence();
    }

    public void AddToScore(int points)
    {
        currentScore += points;
        Debug.Log($"Pegging points awarded: {points}. Total pegging score: {currentScore}");
        UpdateScoreDisplay();
    }

    private void ResetSequence()
    {
        runningTotal = 0;
        recentRanks.Clear();
        UpdateCountDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    private void UpdateCountDisplay()
    {
        if (countText != null)
        {
            countText.text = "Count: " + runningTotal;
        }
        else
        {
            Debug.Log($"Pegging Count: {runningTotal}");
        }
    }

    private static int GetRank(CardUIController cardUI)
    {
        // Accepts either numeric strings ("1".."13") or face values ("A","J","Q","K")
        string s = cardUI?.number?.ToString()?.Trim();
        if (string.IsNullOrEmpty(s)) return 0;

        if (int.TryParse(s, out int n))
        {
            return Mathf.Clamp(n, 1, 13);
        }

        switch (s.ToUpperInvariant())
        {
            case "A": return 1;
            case "J": return 11;
            case "Q": return 12;
            case "K": return 13;
            default:
                Debug.LogWarning($"Unknown card rank '{s}' on {cardUI?.name}. Defaulting to 0.");
                return 0;
        }
    }

    private static int GetPeggingValue(int rank)
    {
        if (rank >= 11) return 10; // J/Q/K
        return Mathf.Min(rank, 10); // A=1, 2..10=pip
    }

    // Compute the longest run ending at the tail with all distinct ranks; minimum length 3
    private static int ComputeRunPointsFromTail(List<int> ranks)
    {
        int maxRun = 0;
        var seen = new HashSet<int>();
        int min = int.MaxValue;
        int max = int.MinValue;

        for (int i = ranks.Count - 1; i >= 0; i--)
        {
            int r = ranks[i];
            if (seen.Contains(r)) break; // duplicates break run search window
            seen.Add(r);
            if (r < min) min = r;
            if (r > max) max = r;

            int len = seen.Count;
            if (len >= 3 && (max - min + 1) == len)
            {
                if (len > maxRun) maxRun = len;
            }
        }

        return maxRun; // points = length of the run
    }
}
