using UnityEngine;
using System.Collections.Generic;
using Smooth.Slinq;

public static class UnitUtil
{
	public static bool IsBranch(Dictionary<TileManager.TileDirection, Tile> movableDictionary)
	{
		int numberOfMovableDirection = 0;
		numberOfMovableDirection = movableDictionary.Count;

		if (numberOfMovableDirection > 1)
		{
			return true;
		}

		if (numberOfMovableDirection == 0)
		{
			Debug.Log("There is no movable tile!");
		}

		return false;
	}

	public static bool IsEnemyEncounter(int tileKey)
	{
		var enemies = GameManager.gameManagerInstance.GetEnemiesList();

		return Slinqable.Slinq(enemies).Where(
				(enemyManager) => enemyManager.GetCurrentTileKey() == tileKey
			).FirstOrNone().isSome;
	}

	public static bool IsPlayerEncounter(int tileKey)
	{
		var players = GameManager.GetAllPlayersEnumerator();

		return Slinqable.Slinq(players).Where(
				(characterManager) => characterManager.GetCurrentTileKey() == tileKey
			).FirstOrNone().isSome;
	}

	public static string GetEnemyIdOnTile(int tileKey)
	{
		var enemies = GameManager.gameManagerInstance.GetEnemiesList();

		return Slinqable.Slinq(enemies).Where(
				(enemyManager) => enemyManager.GetCurrentTileKey() == tileKey
			).First().enemyId;
	}
}
