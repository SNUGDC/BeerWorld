using UnityEngine;
using System.Collections.Generic;

public class Character : Unit {

	//public List<Item> itemList = new List<Item>();
    int remainBuffOrDebuffTurn = 0;
    int bonusStat = 0;
    int remainJailTurn = 0;
    Tile spawnTile  = null;

    public int GetBonusStat()
    {
        return bonusStat;
    }

    public void UpdateRemainBuffTime()
    {
        if (remainBuffOrDebuffTurn != 0)
        {
            remainBuffOrDebuffTurn--;
            Debug.Log("Remain (de)buff turn : " + remainBuffOrDebuffTurn);
        } else
        {
            bonusStat = 0;
        }

        if (remainJailTurn != 0)
        {
            remainJailTurn--;
            Debug.Log("Remain jail turn : " + remainJailTurn);
        }
    }

    public void InJail()
    {
        //FIXME : After implement turn counting system. 
        remainJailTurn = 3+1;
        Debug.Log("In Jail during 3 turn...");
    }

    public bool IsUnitInJail()
    {
        if (remainJailTurn != 0)
        {
            Debug.Log("Remain " + remainJailTurn + " turn in Jail...");
            return true;
        }
        else 
            return false;
    }

    public void SetBuffOrDeBuff()
    {
        //FIXME : After implement turn counting system.
        remainBuffOrDebuffTurn = 3+1;
        int rollResult = Dice.Roll(BDice.Species.Six);
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

	public void SetStartTile(Tile startTile)
	{
		this.spawnTile = startTile;
	}

    public void CheckSaveTile(int tileKeyOfSaveTile)
    {
        Tile saveTile = TileManager.GetExistTile(tileKeyOfSaveTile);
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
