using UnityEngine;
using System.Collections;

public class BattleResultApplier 
{
	public enum BattleResultState
	{
		PlayerWin,
		EnemyWin,
		Draw,
		None
	}

	public static BattleResultState state = BattleResultState.None;

	public static void EnemyDelete(BattlePlayer enemy)
	{
		Debug.LogWarning("(FIXME: Not implemented yet) Enemy is dead.");
	}

	public static void PlayerRespawn(BattlePlayer player)
	{
		Character characterInstance = GameManager.GetMyCharacterManager().GetCharacterInstance();

		characterInstance.currentHp = characterInstance.maxHp;
		Debug.Log("currentHp : " + characterInstance.currentHp + " maxHp : " + characterInstance.maxHp);

		GameManager.GetMyCharacterManager().InitializeCharacter();
		Debug.Log("Player return to startTile.");
	}

	public static void ApplyBattleResult(BattlePlayer player, BattlePlayer enemy)
	{
		GameManager.GetMyCharacterManager().GetCharacterInstance().currentHp = player.GetHp();

		if(state == BattleResultState.PlayerWin)
		{
			EnemyDelete(enemy);
		}
		else if(state == BattleResultState.EnemyWin)
		{
			PlayerRespawn(player);
		}
		else if (state == BattleResultState.Draw)
		{
			EnemyDelete(enemy);
			PlayerRespawn(player);
		}

		state = BattleResultState.None;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
