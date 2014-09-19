using UnityEngine;
using System.Collections.Generic;

public class Character : Unit {

	int remainBuffOrDebuffTurn = 0;
    int bonusStat = 0;
    int remainJailTurn = 0;
    Tile spawnTile  = null;

    CharClass charClass = CharClass.Novice; 

    List<Item> inventory = new List<Item>();
    public static int MaxInventorySize = 3;

    public enum Item
    {
        DiceChange,
        DiceResultChange,
        Dodge,
        Berserk,
        Block,
        Adding,
//        Revive, //this item can be used only boss battle.
        None
    }

    public void SetCharClass(CharClass charClass)
    {
        this.charClass = charClass;
    }

    public int GetNumberOfItems()
    {
        return inventory.Count;
    }

    public void GetItem(Character.Item newItem)
    {
        inventory.Add(newItem);
        Debug.Log("Get " + newItem + " / Inventory : " + GetNumberOfItems() + " / 3");
    }

    public void UseItem(Character.Item item)
    {
        inventory.Remove(item);
        Debug.Log("Using " + item);
    }

    public enum CharClass
    {
        Novice,
        Warrior,
        Tanker,
        Attacker,
        Thief
    }

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
        remainJailTurn = 1+1;
        Debug.Log("In Jail during 1 turn...");
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

    void SetWarriorStats()
    {
        maxHp = 3;
        
        numberOfAttackDice = 2;
        speciesOfAttackDice = BDice.Species.Six;
        
        numberOfDefenseDice = 2;
        speciesOfDefenseDice = BDice.Species.Six;
        
        numberOfMoveDice = 1;
        speciesOfMoveDice = BDice.Species.Six;
    }

    void SetTankerStats()
    {
        maxHp = 3;
        
        numberOfAttackDice = 1;
        speciesOfAttackDice = BDice.Species.Six;
        
        numberOfDefenseDice = 3;
        speciesOfDefenseDice = BDice.Species.Six;
        
        numberOfMoveDice = 1;
        speciesOfMoveDice = BDice.Species.Six;
    }

    void SetAttackerStats()
    {
        maxHp = 3;
        
        numberOfAttackDice = 3;
        speciesOfAttackDice = BDice.Species.Six;
        
        numberOfDefenseDice = 1;
        speciesOfDefenseDice = BDice.Species.Six;
        
        numberOfMoveDice = 1;
        speciesOfMoveDice = BDice.Species.Six;
    }

    void SetDefaultStats()
    {
        maxHp = 3;
        
        numberOfAttackDice = 1;
        speciesOfAttackDice = BDice.Species.Six;
        
        numberOfDefenseDice = 1;
        speciesOfDefenseDice = BDice.Species.Six;
        
        numberOfMoveDice = 1;
        speciesOfMoveDice = BDice.Species.Six;
    }

    void SetStats()
    {
        if (charClass == CharClass.Warrior)
        {
            SetWarriorStats();
        } 
        else if (charClass == CharClass.Tanker)
        {
            SetTankerStats();
        } 
        else if (charClass == CharClass.Attacker)
        {
            SetAttackerStats();
        } 
        else
        {
            SetDefaultStats();
        }
    }

	// Use this for initialization = BDice.Species.Six;
	void Start ()
	{
        SetStats();
        currentHp = maxHp;
	}
}
