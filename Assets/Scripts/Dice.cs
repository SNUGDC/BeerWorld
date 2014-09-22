using UnityEngine;
using System.Collections;

public class Dice
{
	public static int Roll(BDice.Species diceSpecies)
	{
		if (diceSpecies == BDice.Species.Four)
		{
			var result =  BattleManager.random.Next(1, 5);
			if (result < 1 && 4 < result) {
				Debug.LogError("4DIce is " + result);
			}
			return result;
		}
		else if (diceSpecies == BDice.Species.Six)
		{
			var result = BattleManager.random.Next(1, 7);
			if (result < 1 && 6 < result) {
				Debug.LogError("6DIce is " + result);
			}
			return result;
		}
		else if (diceSpecies == BDice.Species.One)
		{
			return 1;
		}
		else
		{
			Debug.Log("Dice is nothing.");
			return 0;
		}
	}
}
