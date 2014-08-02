using UnityEngine;
using System.Collections.Generic;

public class EnemyPlaceHolder
{
    private Enemy enemyPrefab;

    private List<int> enemyPaces = new List<int>();

    public EnemyPlaceHolder(Enemy enemyPrefab)
    {
        this.enemyPrefab = enemyPrefab;

        enemyPaces.Add(FieldTileUtility.GetKeyFromCoord(5,5));
    }

		private EnemyManager enemyManager;

    public void PlaceEnemy(Enemy.EnemyType enemyType)
    {
        Debug.Log("Place enemy.");
				Tile initialTile = TileManager.GetExistTile(enemyPaces[0]);
				enemyManager = EnemyManager.Create(enemyPrefab, null, initialTile);
				enemyManager.Init();
    }

		public EnemyManager getEnemyManager() {
			return enemyManager;
		}
}
