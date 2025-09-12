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

    // Add to Pegging.cs
    public int ScoreFinalHand(List<CardUIController> hand, CardUIController starterCard)
    {
        // Combine hand and starter for scoring
        List<CardUIController> allCards = new List<CardUIController>(hand);
        allCards.Add(starterCard);

        int points = 0;
        points += CountFifteens(allCards) * 2;
        points += CountPairs(allCards);
        points += CountRuns(allCards);
        points += CountFlush(hand, starterCard);
        points += CountNobs(hand, starterCard);

        AddToScore(points);
        return points;
    }

    // Count all unique combinations of cards that sum to 15
    private int CountFifteens(List<CardUIController> cards)
    {
        int count = 0;
        int n = cards.Count;
        int[] values = new int[n];
        for (int i = 0; i < n; i++)
        {
            int rank = GetRank(cards[i]);
            values[i] = GetPeggingValue(rank);
        }

        // Check all combinations (2 to n cards)
        for (int size = 2; size <= n; size++)
        {
            int[] indices = new int[size];
            System.Action<int, int> comb = null;
            comb = (start, depth) =>
            {
                if (depth == size)
                {
                    int sum = 0;
                    for (int j = 0; j < size; j++) sum += values[indices[j]];
                    if (sum == 15) count++;
                    return;
                }
                for (int i = start; i < n; i++)
                {
                    indices[depth] = i;
                    comb(i + 1, depth + 1);
                }
            };
            comb(0, 0);
        }
        return count;
    }

    // Count pairs, 3-of-a-kind, 4-of-a-kind (pairs = 2 pts, 3-of-a-kind = 6 pts, 4-of-a-kind = 12 pts)
    private int CountPairs(List<CardUIController> cards)
    {
        int points = 0;
        int n = cards.Count;
        Dictionary<int, int> rankCounts = new Dictionary<int, int>();
        for (int i = 0; i < n; i++)
        {
            int rank = GetRank(cards[i]);
            if (!rankCounts.ContainsKey(rank)) rankCounts[rank] = 0;
            rankCounts[rank]++;
        }
        foreach (var kvp in rankCounts)
        {
            int count = kvp.Value;
            if (count == 2) points += 2;
            else if (count == 3) points += 6;
            else if (count == 4) points += 12;
        }
        return points;
    }

    // Count runs (sequences of 3 or more consecutive ranks, each run scores its length)
    private int CountRuns(List<CardUIController> cards)
    {
        int maxRun = 0;
        int n = cards.Count;
        List<int> ranks = new List<int>();
        for (int i = 0; i < n; i++) ranks.Add(GetRank(cards[i]));
        ranks.Sort();

        // Check all combinations of 3 or more cards
        for (int size = 3; size <= n; size++)
        {
            int[] indices = new int[size];
            System.Action<int, int> comb = null;
            comb = (start, depth) =>
            {
                if (depth == size)
                {
                    List<int> runRanks = new List<int>();
                    for (int j = 0; j < size; j++) runRanks.Add(ranks[indices[j]]);
                    runRanks.Sort();
                    bool isRun = true;
                    for (int k = 1; k < runRanks.Count; k++)
                    {
                        if (runRanks[k] != runRanks[k - 1] + 1)
                        {
                            isRun = false;
                            break;
                        }
                    }
                    if (isRun) maxRun = Mathf.Max(maxRun, size);
                    return;
                }
                for (int i = start; i < n; i++)
                {
                    indices[depth] = i;
                    comb(i + 1, depth + 1);
                }
            };
            comb(0, 0);
        }
        return maxRun;
    }

    // Count flushes (4 points for 4-card flush, 5 for 5-card flush including starter)
    private int CountFlush(List<CardUIController> hand, CardUIController starterCard)
    {
        if (hand.Count < 4) return 0;
        string suit = hand[0].suitNumber;
        bool allSame = true;
        for (int i = 1; i < hand.Count; i++)
        {
            if (hand[i].suitNumber != suit)
            {
                allSame = false;
                break;
            }
        }
        if (!allSame) return 0;
        // 4-card flush
        int points = 4;
        // 5-card flush (starter matches)
        if (starterCard != null && starterCard.suitNumber == suit) points = 5;
        return points;
    }

    // Count nobs (Jack in hand matches starter's suit)
    private int CountNobs(List<CardUIController> hand, CardUIController starterCard)
    {
        if (starterCard == null) return 0;
        foreach (var card in hand)
        {
            int rank = GetRank(card);
            if (rank == 11 && card.suitNumber == starterCard.suitNumber)
            {
                return 1;
            }
        }
        return 0;
    }
}
