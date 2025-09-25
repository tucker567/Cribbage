using UnityEngine;

public enum PegRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "15Peg", menuName = "Pegs/15 Peg")]
public class FifteenPeg : PegDefinition
{
    public FifteenPeg()
    {
        pegName = "15 Peg";
        description = "Gives +3 points for every fifteen.";
        cost = 10;
        PegRarity rarity = PegRarity.Common;
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

[CreateAssetMenu(fileName = "PairPeg", menuName = "Pegs/Pair Peg")]
public class PairPeg : PegDefinition
{
    public PairPeg()
    {
        pegName = "Pair Peg";
        description = "Gives +2 points for every pair.";
        cost = 8;
        PegRarity rarity = PegRarity.Common;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "PairBonus",
                description = "Adds +2 points for every pair.",
                value = 2
            }
        };
    }
}

[CreateAssetMenu(fileName = "RunPeg", menuName = "Pegs/Run Peg")]
public class RunPeg : PegDefinition
{
    public RunPeg()
    {
        pegName = "Run Peg";
        description = "Gives +4 points for every run.";
        cost = 12;
        PegRarity rarity = PegRarity.Common;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "RunBonus",
                description = "Adds +4 points for every run.",
                value = 4
            }
        };
    }
}

[CreateAssetMenu(fileName = "FacePeg", menuName = "Pegs/Face Peg")]
public class FacePeg : PegDefinition
{
    public FacePeg()
    {
        pegName = "Face Peg";
        description = "Gives +5 points for every face card.";
        cost = 15;
        PegRarity rarity = PegRarity.Rare;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "FaceBonus",
                description = "Adds +5 points for every face card.",
                value = 5
            }
        };
    }
}

[CreateAssetMenu(fileName = "DarkPeg", menuName = "Pegs/Dark Peg")]
public class DarkPeg : PegDefinition
{
    public DarkPeg()
    {
        pegName = "Dark Peg";
        description = "50% chance to gain +8 50% chance to lose -10 points";
        cost = 18;
        PegRarity rarity = PegRarity.Epic;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "DarkBonus",
                description = "50% chance to gain +8 50% chance to lose -10 points",
                value = 6
            }
        };
    }
}

[CreateAssetMenu(fileName = "LuckyPeg", menuName = "Pegs/Lucky Peg")]
public class LuckyPeg : PegDefinition
{
    public LuckyPeg()
    {
        pegName = "Lucky Peg";
        description = "Gives +7 points for every lucky card (7s).";
        cost = 14;
        PegRarity rarity = PegRarity.Rare;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "LuckyBonus",
                description = "Adds +7 points for every lucky card (7s).",
                value = 7
            }
        };
    }
}

[CreateAssetMenu(fileName = "RememberyPeg", menuName = "Pegs/Remembery Peg")]
public class RememberyPeg : PegDefinition
{
    public RememberyPeg()
    {
        pegName = "Remembery Peg";
        description = "Gives +10 if you score all or more of the same type of points as the previous round.";
        cost = 20;
        PegRarity rarity = PegRarity.Epic;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "RememberyBonus",
                description = "Gives +10 if you score all or more of the same type of points as the previous round.",
                value = 10
            }
        };
    }
}

[CreateAssetMenu(fileName = "SteadyPeg", menuName = "Pegs/Steady Peg")]
public class SteadyPeg : PegDefinition
{
    public SteadyPeg()
    {
        pegName = "Steady Peg";
        description = "Gives +1 point for every card in your hand that is not a face card.";
        cost = 6;
        PegRarity rarity = PegRarity.Common;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "SteadyBonus",
                description = "Adds +1 point for every card in your hand that is not a face card.",
                value = 1
            }
        };
    }
}

[CreateAssetMenu(fileName = "LuckyUnluckyPeg", menuName = "Pegs/Lucky Unlucky Peg")]
public class LuckyUnluckyPeg : PegDefinition
{
    public LuckyUnluckyPeg()
    {
        pegName = "Lucky Unlucky Peg";
        description = "If last hand scored less than 5 points, double next hand's";
        cost = 10;
        PegRarity rarity = PegRarity.Rare;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "LuckyUnluckyBonus",
                description = "Adds +5 points for every lucky card (7s) and -5 points for every unlucky card (2s).",
                value = 5
            }
        };
    }
}

[CreateAssetMenu(fileName = "RubyPeg", menuName = "Pegs/Ruby Peg")]
public class RubyPeg : PegDefinition
{
    public RubyPeg()
    {
        pegName = "Ruby Peg";
        description = "Gives +1 points for every red card.";
        cost = 6;
        PegRarity rarity = PegRarity.Common;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "RedBonus",
                description = "Adds +1 points for every red card.",
                value = 1
            }
        };
    }
}

[CreateAssetMenu(fileName = "ShadePeg", menuName = "Pegs/Shade Peg")]
public class ShadePeg : PegDefinition
{
    public ShadePeg()
    {
        pegName = "Shade Peg";
        description = "Gives +2 points for every black card.";
        cost = 8;
        PegRarity rarity = PegRarity.Common;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "ShadeBonus",
                description = "Adds +2 points for every black card.",
                value = 2
            }
        };
    }
}

[CreateAssetMenu(fileName = "MimicPeg", menuName = "Pegs/Mimic Peg")]
public class MimicPeg : PegDefinition
{
    public MimicPeg()
    {
        pegName = "Mimic Peg";
        description = "Mimics the effect of a random peg.";
        cost = 12;
        PegRarity rarity = PegRarity.Rare;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "MimicBonus",
                description = "Mimics the effect of a random peg.",
                value = 0
            }
        };
    }
}

[CreateAssetMenu(fileName = "HoarderPeg", menuName = "Pegs/Hoarder Peg")]
public class HoarderPeg : PegDefinition
{
    public HoarderPeg()
    {
        pegName = "Hoarder Peg";
        description = "Increases hand size by 1.";
        cost = 8;
        PegRarity rarity = PegRarity.Common;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "HoarderBonus",
                description = "Increases hand size by 1.",
                value = 1
            }
        };
    }
}

[CreateAssetMenu(fileName = "ExtraPeg", menuName = "Pegs/Extra Peg")]
public class ExtraPeg : PegDefinition
{
    public ExtraPeg()
    {
        pegName = "Extra Peg";
        description = "Pull two extra cards from the deck for your played cards.";
        cost = 20;
        PegRarity rarity = PegRarity.Epic;
        effects = new PegEffect[]
        {
            new PegEffect
            {
                effectName = "ExtraBonus",
                description = "Pull two extra cards from the deck for your played cards.",
                value = 2
            }
        };
    }
}