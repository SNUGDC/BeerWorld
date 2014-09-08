using UnityEngine;
using System.Collections.Generic;

public class CalculationResult
{
    public readonly List<int> diceResults;
    public readonly int totalDiceResult;

    public CalculationResult(List<int> diceResults, int totalDiceResult)
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

public class BattleCalculator
{
    public CalculationResult GetAttackDiceResult(BattlePlayer player)
    {
        // calculate dice.
        // needed : Dice Info <- player.
        List<BDice.Species> attackDices = player.attackDices;

        List<int> diceResults = new List<int>();
        int totalDiceResult = 0;

        for (int i = 0; i < attackDices.Count; i++)
        {
            int diceResult = Dice.Roll(attackDices[i]);

            diceResults.Add(diceResult);
            totalDiceResult += diceResult;
        }

        totalDiceResult += player.bonusStat;
        if (player.bonusStat != 0)
        {
            Debug.Log("bonus stat " + player.bonusStat + " added");
        }

        return new CalculationResult(diceResults, totalDiceResult);
    }

    public CalculationResult GetDefenseDiceResult(BattlePlayer player)
    {
        List<BDice.Species> defenseDices = player.defenseDices;

        List<int> diceResults = new List<int>();
        int totalDiceResult = 0;

        for (int i = 0; i < defenseDices.Count; i++)
        {
            int diceResult = Dice.Roll(defenseDices[i]);

            diceResults.Add(diceResult);
            totalDiceResult += diceResult;
        }

        totalDiceResult += player.bonusStat;
        if (player.bonusStat != 0)
        {
            Debug.Log("bonus stat " + player.bonusStat + " added");
        }

        return new CalculationResult(diceResults, totalDiceResult);
    }
}
