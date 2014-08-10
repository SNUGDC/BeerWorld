using UnityEngine;
using System.Collections;

public partial class NetworkManager : MonoBehaviour
{

	public static void StartBattle(NetworkViewID user, string enemy, int seed)
	{
		networkInstance.networkView.RPC("ReceiveStartBattle", RPCMode.All, user, enemy, seed);
	}

	[RPC]
	private void ReceiveStartBattle(NetworkViewID user, string enemyId, int seed)
	{
		Random.seed = seed;

		UnitManager player = null;
		if (user.isMine)
		{
			player = GameManager.GetMyCharacterManager();
		}
		else
		{
			player = GameManager.GetCharacterManager(user);
		}

		EnemyManager enemy = GameManager.gameManagerInstance.GetEnemy(enemyId).ValueOr((EnemyManager)null);

		BattleManager.battleManagerInstance.ShowBattle(player, enemy, user.isMine);
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
