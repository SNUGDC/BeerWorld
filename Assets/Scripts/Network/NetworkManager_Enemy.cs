using UnityEngine;
using System;
using System.Collections;

public partial class NetworkManager : MonoBehaviour
{
    public static void MakeEnemy(EnemyInfo enemyInfo)
    {
        Guid enemyGuid = Guid.NewGuid();
        if (enemyInfo.enemyType == Enemy.EnemyType.Smallest)
        {
            networkInstance.networkView.RPC("ReceiveMakeSmallestEnemy",
                    RPCMode.All, enemyGuid.ToString(), enemyInfo.enemyPlaceTileKey);
        } 
        else if (enemyInfo.enemyType == Enemy.EnemyType.Middle)
        {
            networkInstance.networkView.RPC("ReceiveMakeMiddleEnemy",
                    RPCMode.All, enemyGuid.ToString(), enemyInfo.enemyPlaceTileKey);
        }
        else {
            networkInstance.networkView.RPC("ReceiveMakeNonetypeEnemy",
                    RPCMode.All, enemyGuid.ToString(), enemyInfo.enemyPlaceTileKey);
        }
    }

	[RPC]
	private void ReceiveMakeSmallestEnemy(string enemyId, int tileKey)
	{
        Enemy.EnemyType type = Enemy.EnemyType.Smallest;
        GameManager.gameManagerInstance.InstantiateEnemyByNetwork(enemyId, tileKey, type);
	}

    [RPC]
    private void ReceiveMakeMiddleEnemy(string enemyId, int tileKey)
    {
        Enemy.EnemyType type = Enemy.EnemyType.Middle;
        GameManager.gameManagerInstance.InstantiateEnemyByNetwork(enemyId, tileKey, type);
    }

    [RPC]
    private void ReceiveMakeNonetypeEnemy(string enemyId, int tileKey)
    {
        Enemy.EnemyType type = Enemy.EnemyType.None;
        GameManager.gameManagerInstance.InstantiateEnemyByNetwork(enemyId, tileKey, type);
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
