using UnityEngine;
using System.Collections;

public class Dice {

//    private static Dice instance = null;
//	private int diceResult = 0;
//
//	public static int getDiceResult()
//    {
//        return instance.diceResult;
//    }
//
//    void Awake () {
//        instance = this;
//    }

	public static int Roll() {
        int diceResult = Random.Range(1, 6);
        //return diceResult;
        return 2;
	}
}
