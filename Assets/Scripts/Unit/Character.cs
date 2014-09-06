using UnityEngine;
using System.Collections.Generic;

public class Character : Unit {

	//public List<Item> itemList = new List<Item>();
    int remainBuffOrDebuffTurn = 0;
    int bonusStat = 0;
    int remainJailTurn = 0;
    Tile spawnTile = TileManager.GetStartTile();

    public void InJail()
    {
        remainJailTurn = 3;
    }

    public bool IsUnitInJail()
    {
        if (remainJailTurn != 0)
            return true;
        else 
            return false;
    }

    public void SetBuffOrDeBuff()
    {
        remainBuffOrDebuffTurn = 3;
        int rollResult = Dice.Roll();
        if (rollResult <= 3)
        {
            bonusStat = rollResult;
            Debug.Log("Get buff! Bonus stat " + bonusStat);
        } 
        else
        {
            bonusStat = (-1) * (rollResult - 3);
            Debug.Log("Get Debuff! Bonus stat " + bonusStat);
        }
    }

    public Tile GetSpawnTile()
    {
        return spawnTile;
    }

    public void CheckSaveTile(Tile saveTile)
    {
        spawnTile = saveTile;
        Debug.Log("Save at this check point!");
    }

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
