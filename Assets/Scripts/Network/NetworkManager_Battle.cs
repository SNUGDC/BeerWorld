using System;
using UnityEngine;
using System.Collections;

public partial class NetworkManager : MonoBehaviour
{
	public static void StartBattle(string enemy)
	{
		int seed = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);
		var userId = NetworkManager.networkInstance.Id;
		networkInstance.networkView.RPC("ReceiveStartBattle", RPCMode.All, userId, enemy, seed, (int)BattleManager.AttackOrDefense.Attack);
	}

	public static void StartBattleFromEnemy(string enemy, NetworkViewID player)
	{
		int seed = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);
		networkInstance.networkView.RPC("ReceiveStartBattle", RPCMode.All, player, enemy, seed, (int)BattleManager.AttackOrDefense.Defense);
	}

	[RPC]
	private void ReceiveStartBattle(NetworkViewID user, string enemyId, int seed, int attackOrDefense)
	{
		UnityEngine.Random.seed = seed;

		CharacterManager player = null;
		if (user.isMine)
		{
			player = GameManager.GetMyCharacterManager();
		}
		else
		{
			player = GameManager.GetCharacterManager(user);
		}

		EnemyManager enemy = GameManager.gameManagerInstance.GetEnemy(enemyId).ValueOr((EnemyManager)null);

		BattleManager.battleManagerInstance.ShowBattle(player, enemy, user.isMine, (BattleManager.AttackOrDefense)attackOrDefense);
	}

	public static void BattleRoleDice()
	{
		networkInstance.networkView.RPC("ReceiveBattleRoleDice", RPCMode.All);
	}

	[RPC]
	private void ReceiveBattleRoleDice()
	{
		BattleManager.battleManagerInstance.OnRollClicked();
	}
}
