using UnityEngine;
using System.Collections.Generic;

public class EnemyInfoHolder {

    private List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();

    public int enemyPlaceTileKey;
    public Enemy.EnemyType enemyType; 

    public EnemyInfoHolder()
    {
        //Smallest Enemies.
        enemyInfoList.Add(new EnemyInfo(FieldTileUtility.GetKeyFromCoord(6, 4), Enemy.EnemyType.Smallest));
        enemyInfoList.Add(new EnemyInfo(FieldTileUtility.GetKeyFromCoord(10, 4), Enemy.EnemyType.Smallest));
        enemyInfoList.Add(new EnemyInfo(FieldTileUtility.GetKeyFromCoord(11, 11), Enemy.EnemyType.Smallest));
        
        //Middle Enemies.
        enemyInfoList.Add(new EnemyInfo(FieldTileUtility.GetKeyFromCoord(5, 5), Enemy.EnemyType.Middle));
        enemyInfoList.Add(new EnemyInfo(FieldTileUtility.GetKeyFromCoord(9, 5), Enemy.EnemyType.Middle));
        enemyInfoList.Add(new EnemyInfo(FieldTileUtility.GetKeyFromCoord(10, 11), Enemy.EnemyType.Middle));
    }
    
    public EnemyInfo getFirstEnemyInfo()
    {
        return enemyInfoList[0];
    }
    
    public List<EnemyInfo> GetEnemyInfoList()
    {
        return enemyInfoList;
    }
}
