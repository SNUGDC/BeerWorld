﻿using UnityEngine;
using System.Collections;

public class DiceRollerButton : MonoBehaviour {

    public CharManager characterManager;

	// Use this for initialization
	void Start () {
	    //Instantiate(
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        int diceResult = Dice.Roll();
        Debug.Log("Dice : " + diceResult);
        characterManager.howManyMove = 1;
    }
}