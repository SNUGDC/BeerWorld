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

    public void PlaceEnemy()
    {
        Debug.Log("Place enemy.");
        Enemy enemyInstance = GameObject.Instantiate(enemyPrefab) as Enemy;
        CharacterMover mover = enemyInstance.GetComponent<CharacterMover>();
        mover.MoveTo(enemyPaces[0]);
    }
}
