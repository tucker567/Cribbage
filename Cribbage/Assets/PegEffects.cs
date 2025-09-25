using UnityEngine;

[CreateAssetMenu(fileName = "15Peg", menuName = "Pegs/15 Peg")]
public class FifteenPeg : PegDefinition
{
    public FifteenPeg()
    {
        pegName = "15 Peg";
        description = "Gives +3 points for every fifteen.";
        cost = 10;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "FifteenBonus",
                description = "Adds +3 points for every fifteen.",
                value = 3
            }
        };
    }
}
