using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    public static readonly int Depth = -3;

	public int currentTileKey;
	public int preTileKey;
	public int prePreTileKey;
	
	public int maxHp;
	public int currentHp;

	public int numberOfAttackDice;
	public BDice.Species speciesOfAttackDice;
	
	public int numberOfDefenseDice;
	public BDice.Species speciesOfDefenseDice;
	
	public int numberOfMoveDice;
	public BDice.Species speciesOfMoveDice;
	
	//public List<Item> itemList = new List<Item>();


	// Use this for initialization = BDice.Species.Six;
	void Start () 
	{
		//temporary values.
		maxHp = 20;
		currentHp = maxHp;

		numberOfAttackDice = 1;
		speciesOfAttackDice = BDice.Species.Six;

		numberOfDefenseDice = 2;
		speciesOfDefenseDice = BDice.Species.Six;

		numberOfMoveDice = 1;
		speciesOfMoveDice = BDice.Species.Six;
	}
	
	// Update is called once per frameb
	void Update () 
	{
	
	}
}
