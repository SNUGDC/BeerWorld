using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
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

	void Start()
	{

	}

	void Update()
	{

	}

}