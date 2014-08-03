using UnityEngine;
using System.Collections.Generic;

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
}
