using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitDef : ScriptableObject
{
	public int maxHp;

	public int numberOfAttackDice;
	public BDice.Species speciesOfAttackDice;

	public int numberOfDefenseDice;
	public BDice.Species speciesOfDefenseDice;

	public int numberOfMoveDice;
	public BDice.Species speciesOfMoveDice;

	public Texture2D texture;
}
