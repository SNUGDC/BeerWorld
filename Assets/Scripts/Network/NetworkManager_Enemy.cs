using UnityEngine;
using System;
using System.Collections;

public partial class NetworkManager : MonoBehaviour
{
	public static void MakeEnemy(int startTileKey)
	{
		Guid enemyGuid = Guid.NewGuid();
		networkInstance.networkView.RPC("ReceiveMakeEnemy",
				RPCMode.All, enemyGuid.ToString(), startTileKey);
	}

	[RPC]
	private void ReceiveMakeEnemy(string enemyId, int tileKey)
	{
		GameManager.gameManagerInstance.InstantiateEnemyByNetwork(enemyId, tileKey);
	}

	public static void MoveEnemy(int moveTileKey, string enemyId)
	{
		networkInstance.networkView.RPC("ReceiveMoveEnemy",
				RPCMode.Others, moveTileKey, enemyId);
	}

	[RPC]
	private void ReceiveMoveEnemy(int moveTileKey, string enemyId)
	{
		GameManager.gameManagerInstance.MoveEnemy(moveTileKey, enemyId);
	}
}
