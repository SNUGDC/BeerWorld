using System;
using UnityEngine;
using System.Collections;

public class BattleStartButton : MonoBehaviour
{
	public BattleManager battleManager;

	void OnMouseDown()
	{
		var enemyId = GameManager.gameManagerInstance.GetFirstEnemyId();
		var seed = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);

		UnityEngine.Random.seed = seed;

		enemyId.ForEachOr(
				enemy => NetworkManager.StartBattle(NetworkManager.networkInstance.Id, enemy, seed),
				() => Debug.Log("There is no enemy"));
	}
}
