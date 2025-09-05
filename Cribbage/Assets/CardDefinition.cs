using UnityEngine;


[CreateAssetMenu(fileName = "NewCardDefinition", menuName = "Cards/Card Definition")]
public class CardDefinition : ScriptableObject
{
    public string cardName;
    public Sprite artwork;
    public Sprite backArtwork;
    public string number;
    public string suitNumber;
    // Spades = 1, Clubs = 2, Diamonds = 3, Hearts = 4
}
