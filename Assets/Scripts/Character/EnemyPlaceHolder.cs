using UnityEngine;
using System.Collections.Generic;

public class EnemyPlaceHolder
{
    private Enemy enemyPrefab;

    private List<int> enemyPlaces = new List<int>();

    public EnemyPlaceHolder(Enemy enemyPrefab)
    {
        this.enemyPrefab = enemyPrefab;

        enemyPlaces.Add(FieldTileUtility.GetKeyFromCoord(5,5));
    }

    public void PlaceEnemy(Enemy.EnemyType enemyType)
    {
        Debug.Log("Place enemy.");
        Enemy enemyInstance = GameObject.Instantiate(enemyPrefab) as Enemy;
        enemyInstance.SetEnemyType(enemyType);
        CharacterMover mover = enemyInstance.GetComponent<CharacterMover>();
        mover.MoveTo(enemyPlaces[0]);
    }
}
