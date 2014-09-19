using UnityEngine;
using System.Collections;

public class Dice
{
	public static int Roll(BDice.Species diceSpecies)
	{
		if (diceSpecies == BDice.Species.Four)
		{
			return BattleManager.random.Next(1, 5);
		}
		else if (diceSpecies == BDice.Species.Six)
		{
			return BattleManager.random.Next(1, 7);
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
