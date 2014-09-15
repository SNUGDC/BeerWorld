using UnityEngine;
using System.Collections.Generic;

public class EnemyInfo : MonoBehaviour {

    public int enemyPlaceTileKey;
    public Enemy.EnemyType enemyType; 

    public EnemyInfo (int tileKey, Enemy.EnemyType type)
    {
        enemyPlaceTileKey = tileKey;
        enemyType = type;
    }
}
