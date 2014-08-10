using UnityEngine;
using System.Collections.Generic;

public class EnemyPlaceHolder
{
  private List<int> enemyPlaces = new List<int>();

  public EnemyPlaceHolder()
  {
    enemyPlaces.Add(FieldTileUtility.GetKeyFromCoord(5,5));
    enemyPlaces.Add(FieldTileUtility.GetKeyFromCoord(9, 5));
    enemyPlaces.Add(FieldTileUtility.GetKeyFromCoord(10, 11));
  }

  public int getEnemyStartTileKey()
  {
    return enemyPlaces[0];
  }

  public List<int> GetEnemyPlaces()
  {
    return enemyPlaces;
  }
}
