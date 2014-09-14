using System;
using UnityEngine;
using System.Collections;

public class BattleStartButton : MonoBehaviour
{
	public BattleManager battleManager;

	void OnMouseDown()
	{
		var enemyId = GameManager.gameManagerInstance.GetFirstEnemyId();
		enemyId.ForEachOr(
				enemy => NetworkManager.StartBattle(enemy),
				() => Debug.Log("There is no enemy"));
	}
}
