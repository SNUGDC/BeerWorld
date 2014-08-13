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

	public static void EnemyDelete(EnemyManager enemy)
	{
		GameManager.gameManagerInstance.KillEnemy(enemy.enemyId);
	}

	public static void PlayerRespawn(UnitManager playerManager)
	{
		playerManager.Die();
	}

	public static void ApplyBattleResult(BattlePlayer player, BattlePlayer enemy, UnitManager playerManager, EnemyManager enemyManager)
	{
		playerManager.GetUnitInstance().currentHp = player.GetHp();
		enemyManager.GetUnitInstance().currentHp = enemy.GetHp();

		if(state == BattleResultState.PlayerWin)
		{
			EnemyDelete(enemyManager);
			playerManager.BattleWin();
		}
		else if(state == BattleResultState.EnemyWin)
		{
			PlayerRespawn(playerManager);
		}
		else if (state == BattleResultState.Draw)
		{
			EnemyDelete(enemyManager);
			PlayerRespawn(playerManager);
		}

		state = BattleResultState.None;
	}
}
