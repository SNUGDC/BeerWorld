using UnityEngine;
using System.Collections;

public class BattleResult 
{
	public static void EnemyDelete(BattlePlayer enemy)
	{

	}

	public static void PlayerRespawn(BattlePlayer player)
	{

	}

	public static void UpdateResult(BattlePlayer player, BattlePlayer enemy)
	{
		GameManager.GetMyCharacterManager().GetCharacterInstance().currentHp = player.GetHp();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
