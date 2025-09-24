using UnityEngine;

[CreateAssetMenu(fileName = "NewPegDefinition", menuName = "Pegs/Peg Definition")]
public class PegDefinition : ScriptableObject
{
    public string pegName;
    public Sprite artwork;
    public string description;
    public int cost; // Cost in the shop
    public PegEffect[] effects; // Array of effects this peg provides
}

[System.Serializable]
public class PegEffect
{
    public string effectName;
    public string description;
    public int value; // Example: +1 point, +1 card, etc.
}