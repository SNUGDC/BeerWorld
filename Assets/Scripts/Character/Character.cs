using UnityEngine;
using System.Collections.Generic;

public class Character : Unit {

	//public List<Item> itemList = new List<Item>();

	// Use this for initialization = BDice.Species.Six;
	void Start ()
	{
		//temporary values.
		maxHp = 10;
		currentHp = maxHp;

		numberOfAttackDice = 1;
		speciesOfAttackDice = BDice.Species.Six;

		numberOfDefenseDice = 1;
		speciesOfDefenseDice = BDice.Species.Six;

		numberOfMoveDice = 1;
		speciesOfMoveDice = BDice.Species.Six;
	}
}
