using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

	public static readonly int Depth = -3;

<<<<<<< HEAD
	public int currentTileKey;
	public int preTileKey;
	public int prePreTileKey;

=======
	public int currentTileKey
	{
		get;
		private set;
	}

	public int preTileKey
	{
		get;
		private set;
	}

	public int prePreTileKey
	{
		get;
		private set;
	}
	
>>>>>>> 8e2d5a9fd3b934fcc58f9cab0aad2d851e2e1966
	public int maxHp;
	public int currentHp;

	public int numberOfAttackDice;
	public BDice.Species speciesOfAttackDice;

	public int numberOfDefenseDice;
	public BDice.Species speciesOfDefenseDice;

	public int numberOfMoveDice;
	public BDice.Species speciesOfMoveDice;

	public List<Item> itemList = new List<Item>();


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

<<<<<<< HEAD
	// Update is called once per frameb
	void Update ()
	{

=======
	public void InitializeTileKey(int tileKey)
	{
		currentTileKey = tileKey;
		prePreTileKey = 0;
		preTileKey = 0;
	}

	/* Can be called with SendMessage */
	public void UpdateTileKey(int tileKey)
	{
		prePreTileKey = preTileKey;
		preTileKey = currentTileKey;
		currentTileKey = tileKey;
>>>>>>> 8e2d5a9fd3b934fcc58f9cab0aad2d851e2e1966
	}
}
