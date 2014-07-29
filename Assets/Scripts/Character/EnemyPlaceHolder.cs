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

    public enum EnemyType
    {
        Smallest,
        None
    }

    EnemyType enemyType;

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

    public void PlaceEnemy(EnemyType enemyType)
    {
        Debug.Log("Place enemy.");
        this.enemyType = enemyType;
        Enemy enemyInstance = GameObject.Instantiate(enemyPrefab) as Enemy;
        enemyInstance.holder = this;
        CharacterMover mover = enemyInstance.GetComponent<CharacterMover>();
        mover.MoveTo(enemyPaces[0]);
    }
}
