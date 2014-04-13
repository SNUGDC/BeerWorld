using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

    private static Dice instance = null;
	private int diceResult = 0;

	public static int getDiceResult()
    {
        return instance.diceResult;
    }

    void Awake () {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Roll() {
        diceResult = Random.Range(1, 6);
        Debug.Log("Dice Result is " + diceResult);
	}
}
