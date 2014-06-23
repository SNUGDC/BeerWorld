using UnityEngine;
using System.Collections.Generic;

public class CalculationResult
{
    public readonly List<int> diceResults;
    public readonly int totalDamage;

    public CalculationResult(List<int> diceResults, int totalDamage)
    {
        this.diceResults = diceResults;
        this.totalDamage = totalDamage;
    }

    public string ToString()
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

        return new CalculationResult(diceResults, totalDiceResult);
        /*int firstDiceResult = 3;
        int secondDiceResult = 5;

        List<int> diceResults = new List<int>();
        diceResults.Add(firstDiceResult);
        diceResults.Add(secondDiceResult);

        return new CalculationResult(
                diceResults, 3 + 5);
*/    }

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

        return new CalculationResult(diceResults, totalDiceResult);

/*        int firstDiceResult = 4;
        int secondDiceResult = 2;

        List<int> diceResults = new List<int>();
        diceResults.Add(firstDiceResult);
        diceResults.Add(secondDiceResult);

        return new CalculationResult(
                diceResults, 4 + 2);
*/    }
}
