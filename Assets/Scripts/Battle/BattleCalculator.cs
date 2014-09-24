using UnityEngine;
using System.Collections.Generic;

public class CalculationResult
{

    public readonly List<BDiceResult> diceResults;
    public int totalDiceResult;

    public CalculationResult(List<BDiceResult> diceResults, int totalDiceResult)
    {
        this.diceResults = diceResults;
        this.totalDiceResult = totalDiceResult;
    }

    public override string ToString()
    {
        string ret = string.Empty;
        foreach(var result in diceResults)
        {
            ret += result + " + ";
        }

        return ret;
    }
}

[System.Serializable]
public class BDiceResult
{
	public readonly BDice.Species species;
	public readonly int diceValue;

	public BDiceResult(BDice.Species species, int diceValue)
	{
		this.species = species;
		this.diceValue = diceValue;
	}
}

public class BattleCalculator
{
    public CalculationResult GetAttackDiceResult(BattlePlayer player)
    {
        // calculate dice.
        // needed : Dice Info <- player.
        List<BDice.Species> attackDices = player.attackDices;

        List<BDiceResult> diceResults = new List<BDiceResult>();
        int totalDiceResult = 0;

        for (int i = 0; i < attackDices.Count; i++)
        {
            int diceResult = Dice.Roll(attackDices[i]);

            diceResults.Add(new BDiceResult(attackDices[i], diceResult));
            totalDiceResult += diceResult;
        }

        if (player.bonusStat != 0)
        {
            Debug.Log("bonus stat " + player.bonusStat + " added");
        }

        return new CalculationResult(diceResults, totalDiceResult);
    }

    public CalculationResult GetDefenseDiceResult(BattlePlayer player)
    {
        List<BDice.Species> defenseDices = player.defenseDices;

        List<BDiceResult> diceResults = new List<BDiceResult>();
        int totalDiceResult = 0;

        for (int i = 0; i < defenseDices.Count; i++)
        {
            int diceResult = Dice.Roll(defenseDices[i]);

            diceResults.Add(new BDiceResult(defenseDices[i], diceResult));
            totalDiceResult += diceResult;
        }

        if (player.bonusStat != 0)
        {
            Debug.Log("bonus stat " + player.bonusStat + " added");
        }

        return new CalculationResult(diceResults, totalDiceResult);
    }
}
