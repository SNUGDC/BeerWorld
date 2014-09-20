using UnityEngine;
using System.Collections.Generic;

public class Character : Unit {

	int _remainBuffOrDebuffTurn;
	int remainBuffOrDebuffTurn
	{
		get
		{
			return _remainBuffOrDebuffTurn;
		}
		set
		{
			ui.SetBuff(bonusStat > 0, _remainBuffOrDebuffTurn);
			_remainBuffOrDebuffTurn = value;
		}
	}

    int bonusStat = 0;
    int remainJailTurn = 0;
	bool isMine = false;
    Tile spawnTile  = null;
	NetworkViewID playerId;
	LeftUIComps.Player ui;

    CharClass charClass = CharClass.Novice; 

    List<Item> inventory = new List<Item>();
    public static int MaxInventorySize = 4;

    public Sprite warriorImg;
    public Sprite tankerImg;
    public Sprite attackerImg;
    public Sprite thiefImg;
    public Sprite noviceImg;

    public Sprite charImg;

    public SpriteRenderer renderer;

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

	public new int currentHp {
		get {
			return base.currentHp;
		}
		set {
			var hp = value;
			if (hp < 0)
			{
				hp = 0;
			}
			ui.SetHp(hp);
			base.currentHp = hp;
		}
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
        Debug.Log("Get " + newItem.ToString() + " / Inventory : " + GetNumberOfItems() + " / 3");
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
            Debug.Log("(De)buff removed");
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
        int rollResult = Dice.Roll(BDice.Species.Six);
        if (rollResult <= 3)
        {
            bonusStat = 1;
            Debug.Log("Get buff! Dice +1 during 3 turns");
        } 
        else
        {
            bonusStat = -1;
            Debug.Log("Get Debuff! Dice -1 durint 3 turns");
        }
        remainBuffOrDebuffTurn = 3+1;
    }

	public Tile GetSpawnTile()
	{
		return spawnTile;
	}

	public void SetStartTile(Tile startTile)
	{
		this.spawnTile = startTile;
	}

	public void SetIsMine(bool isMine)
	{
		this.isMine = isMine;
	}

	public void SetPlayerId(NetworkViewID playerId)
	{
		this.playerId = playerId;
	}

	public void Initialize()
	{
		ui = BattleUIManager.Get().GetPlayerUI(playerId);
	}

    public void CheckSaveTile(int tileKeyOfSaveTile)
    {
        Tile saveTile = TileManager.GetExistTile(tileKeyOfSaveTile);
        spawnTile = saveTile;
        Debug.Log("Save at this check point!");
    }

    void SetWarriorStats()
    {
        charImg = warriorImg;

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
        charImg = tankerImg;

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
        charImg = attackerImg;

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
        charImg = noviceImg;

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

        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = charImg;
	}
}
