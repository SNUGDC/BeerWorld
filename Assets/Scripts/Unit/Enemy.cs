using UnityEngine;
using System.Collections;

public class Enemy : Unit
{
	public enum EnemyType
    {
        Smallest,
        Middle,
        None
    }

   	EnemyType enemyType;

    public void SetEnemyType(EnemyType enemyType)
	{
		this.enemyType = enemyType;
	}

	void SetSmallestEnemyStats()
	{
		Debug.Log("Set Values : Smallest");
		//temporary values.
		maxHp = 2;
		currentHp = maxHp;

		numberOfAttackDice = 2;
		speciesOfAttackDice = BDice.Species.Four;

		numberOfDefenseDice = 2;
		speciesOfDefenseDice = BDice.Species.Four;

		numberOfMoveDice = 1;
		speciesOfMoveDice = BDice.Species.One;
	}

    void SetMiddleEnemyStats()
    {
        Debug.Log("Set Values : Middle");
        //temporary values.
        maxHp = 3;
        currentHp = maxHp;
        
        numberOfAttackDice = 3;
        speciesOfAttackDice = BDice.Species.Four;
        
        numberOfDefenseDice = 3;
        speciesOfDefenseDice = BDice.Species.Four;
        
        numberOfMoveDice = 0;
        speciesOfMoveDice = BDice.Species.Six;
    }

	void Start ()
	{
		if (enemyType == EnemyType.Smallest)
		{
			SetSmallestEnemyStats();
		}
	}
}
