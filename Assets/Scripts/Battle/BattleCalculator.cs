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
        int firstDiceResult = 3;
        int secondDiceResult = 5;

        List<int> diceResults = new List<int>();
        diceResults.Add(firstDiceResult);
        diceResults.Add(secondDiceResult);

        return new CalculationResult(
                diceResults, 3 + 5);
    }

    public CalculationResult GetDefenceDiceResult(BattlePlayer player)
    {
        int firstDiceResult = 4;
        int secondDiceResult = 2;

        List<int> diceResults = new List<int>();
        diceResults.Add(firstDiceResult);
        diceResults.Add(secondDiceResult);

        return new CalculationResult(
                diceResults, 4 + 2);
    }
}
