using UnityEngine;
using System.Collections.Generic;

public class EnemyPlaceHolder
{
    private List<int> enemyPlaces = new List<int>();

    public EnemyPlaceHolder()
    {
        enemyPlaces.Add(FieldTileUtility.GetKeyFromCoord(5,5));
    }

		public int getEnemyStartTileKey()
		{
			return enemyPlaces[0];
		}
}
