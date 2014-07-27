using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public static readonly int Depth = -3;

	public int maxHp;
	public int currentHp;

	public int numberOfAttackDice;
	public BDice.Species speciesOfAttackDice;

	public int numberOfDefenseDice;
	public BDice.Species speciesOfDefenseDice;

	public int numberOfMoveDice;
	public BDice.Species speciesOfMoveDice;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
