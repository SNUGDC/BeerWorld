using UnityEngine;
using System.Collections;

public class Dice {

	public static int Roll(BDice.Species diceSpecies) {
        if (diceSpecies == BDice.Species.Four)
        {
        	return Random.Range(1, 4);	
        }
        else if (diceSpecies == BDice.Species.Six)
        {
            return Random.Range(1, 6);
        }
        else 
        {
        	Debug.Log("Dice is nothing.");
        	return 0;
        }
	}

	public static int Roll()
	{		        
        int diceResult = Random.Range(1, 6);
        return 1;
        //return diceResult;
	}
}
