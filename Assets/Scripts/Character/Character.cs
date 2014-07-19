using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    public static readonly int Depth = -3;

	public int currentTileKey;
	public int preTileKey;
	public int prePreTileKey;
	
	public int HP;

	public int numberOfAttackDice;
	public int speciesOfAttackDice;
	
	public int numberOfDefenseDice;
	public int speciesOfDefenseDice;
	
	public int numberOfMoveDice;
	public int speciesOfMoveDice;
	
	public List<BDice.Species> attackDices = new List<BDice.Species>();
    public List<BDice.Species> defenseDices = new List<BDice.Species>();
    public List<BDice.Species> moveDices = new List<BDice.Species>();

	//public List<Item> itemList = new List<Item>();



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frameb
	void Update () {

	
	}
}
