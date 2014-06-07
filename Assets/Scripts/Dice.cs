using UnityEngine;
using System.Collections;

public class Dice {

	public static int Roll() {
        int diceResult = Random.Range(1, 6);
        return diceResult;
	}
}
