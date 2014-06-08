using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FieldTileUtility : MonoBehaviour {

	public static float TilePixelWidth = 58;
	public static float TilePixelHeight = 64;
    public static float unityTileX = TilePixelWidth / 100;
    public static float unityTileY = TilePixelHeight / 100;

//    [Obsolete("Refactored.")]
//    public static int GetTranslatedTileToKey(Tile tile)
//    {
//        return GetKeyFromTile(tile);
//    }

    public static int GetKeyFromTile(Tile tile)
    {
    	Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);
        Vector2 coordinate = GetTranslatedCoordinate(position.x, position.y);
        int key = GetKeyFromCoord(coordinate);

        return key;
    }

//    [Obsolete("Refactored.")]
//    public static int GetTranslatedCoordinateToKey(Vector2 coordinate)
//    {
//        return GetKeyFromCoord(coordinate);
//    }

    public static int GetKeyFromCoord(Vector2 coordinate)
    {
    	int key = (int)(coordinate.x*100 + coordinate.y);

    	return key;
    }

    [Obsolete("Refactored.")]
    public static Vector2 GetTranslatedKeyToCoordinate (TileManager.TileDirection key, Vector3 standardPositionWithZ)
    {
        return GetCoordOfDirectionByPosition(key, standardPositionWithZ);
    }

    public static Vector2 GetCoordOfDirectionByPosition(TileManager.TileDirection direction, Vector3 basePosition)
    {
    	Vector2 standardPosition = basePosition;
    	Vector2 standardCoordinate = GetTranslatedCoordinate(standardPosition.x, standardPosition.y);
    	Vector2 coordinate = Vector3.zero;
    	if ((int)standardCoordinate.y % 2 == 1) //odd number
    	{
    		if (direction == TileManager.TileDirection.UpLeft) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y + 1);}
    		else if (direction == TileManager.TileDirection.MidLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y);}
    		else if (direction == TileManager.TileDirection.DownLeft) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y - 1);}
    		else if (direction == TileManager.TileDirection.UpRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y + 1);}
    		else if (direction == TileManager.TileDirection.MidRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y);}
    		else if (direction == TileManager.TileDirection.DownRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y - 1);}	
    	}
    	else //even number
    	{
    		if (direction == TileManager.TileDirection.UpLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y + 1);}
    		else if (direction == TileManager.TileDirection.MidLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y);}
    		else if (direction == TileManager.TileDirection.DownLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y - 1);}
    		else if (direction == TileManager.TileDirection.UpRight) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y + 1);}
    		else if (direction == TileManager.TileDirection.MidRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y);}
    		else if (direction == TileManager.TileDirection.DownRight) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y - 1);}
    	}

    	return coordinate;
    }

    public static Vector2 GetPositionFromCoordinate(float coordX, float coordY)
    {
		float posX, posY;
		float zeroIndexX = coordX - 1;
		float zeroIndexY = coordY - 1;
		if (coordY%2 == 1){  //odd number Y
			posX = unityTileX/2+unityTileX * zeroIndexX;
			posY = unityTileY/2+(unityTileY-0.15f) * zeroIndexY;
		}
		else {  //even number y
			posX = unityTileX * zeroIndexX;
			posY = unityTileY/2+(unityTileY-0.15f) * zeroIndexY;
		}		
		return new Vector2(posX, posY);
    }

    [Obsolete("Refactored.")]
	public static Vector2 GetTranslatedPosition(float i, float j){
        return GetPositionFromCoordinate(i, j);
	}

    public static Vector2 GetCoordFromPosition(float posX, float posY)
    {
		float i, j;

		j = (int)((((posY - unityTileY / 2) / (unityTileY - 0.15f)) + 1) + 0.1f);
		i = (int)((posX / unityTileX + 1) + 0.4f);

		return new Vector2(i, j);
    }

    [Obsolete("Refactored.")]
	public static Vector2 GetTranslatedCoordinate(float x, float y){
        return GetCoordFromPosition(x, y);
	}
}
