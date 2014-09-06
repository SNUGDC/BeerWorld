using UnityEngine;
using System.Collections;

public class Enemy : Unit
{
	public enum EnemyType
    {
        Smallest,
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
		maxHp = 5;
		currentHp = maxHp;

		numberOfAttackDice = 2;
		speciesOfAttackDice = BDice.Species.Four;

		numberOfDefenseDice = 1;
		speciesOfDefenseDice = BDice.Species.Four;

		numberOfMoveDice = 1;
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
