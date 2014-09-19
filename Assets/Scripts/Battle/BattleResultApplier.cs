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

	public static void ApplyBattleResult(BattlePlayer player, BattlePlayer enemy, CharacterManager playerManager, EnemyManager enemyManager)
	{
		playerManager.GetCharacterInstance().currentHp = player.GetHp();
		enemyManager.GetEnemyInstance().currentHp = enemy.GetHp();

		if(state == BattleResultState.PlayerWin)
		{
			enemyManager.BattleLose();
			playerManager.BattleWin();
		}
		else if(state == BattleResultState.EnemyWin)
		{
			enemyManager.BattleWin();
			playerManager.BattleLose();
		}
		else if (state == BattleResultState.Draw)
		{
			enemyManager.BattleLose();
			playerManager.BattleLose();
		}

		state = BattleResultState.None;
	}
}
