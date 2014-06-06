using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldTileUtility : MonoBehaviour {

	public static float TilePixelWidth = 58;
	public static float TilePixelHeight = 64;
    public static float unityTileX = TilePixelWidth / 100;
    public static float unityTileY = TilePixelHeight / 100;

    public static int GetTranslatedTileToKey(Tile tile)
    {
    	Vector2 position = new Vector2(tile.transform.position.x, tile.transform.position.y);
        Vector2 coordinate = GetTranslatedCoordinate(position.x, position.y);
        int key = GetTranslatedCoordinateToKey(coordinate);

        return key;
    }

    public static int GetTranslatedCoordinateToKey(Vector2 coordinate)
    {
    	int key = (int)(coordinate.x*100 + coordinate.y);

    	return key;
    }

    public static Vector2 GetTranslatedKeyToCoordinate (TileManager.TileDirection key, Vector3 standardPositionWithZ)
    {
    	Vector2 standardPosition = standardPositionWithZ;
    	Vector2 standardCoordinate = GetTranslatedCoordinate(standardPosition.x, standardPosition.y);
    	Vector2 coordinate = Vector3.zero;
    	if ((int)standardCoordinate.y % 2 == 0) //even number
    	{
    		if (key == TileManager.TileDirection.UpLeft) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y + 1);}
    		else if (key == TileManager.TileDirection.MidLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y);}
    		else if (key == TileManager.TileDirection.DownLeft) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y - 1);}
    		else if (key == TileManager.TileDirection.UpRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y + 1);}
    		else if (key == TileManager.TileDirection.MidRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y);}
    		else if (key == TileManager.TileDirection.DownRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y - 1);}	
    	}
    	else //odd number
    	{
    		if (key == TileManager.TileDirection.UpLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y + 1);}
    		else if (key == TileManager.TileDirection.MidLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y);}
    		else if (key == TileManager.TileDirection.DownLeft) {coordinate = new Vector2 (standardCoordinate.x - 1, standardCoordinate.y - 1);}
    		else if (key == TileManager.TileDirection.UpRight) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y + 1);}
    		else if (key == TileManager.TileDirection.MidRight) {coordinate = new Vector2 (standardCoordinate.x + 1, standardCoordinate.y);}
    		else if (key == TileManager.TileDirection.DownRight) {coordinate = new Vector2 (standardCoordinate.x, standardCoordinate.y - 1);}
    	}

    	return coordinate;
    }

	public static Vector2 GetTranslatedPosition(float i, float j){
		float posX, posY;
		float zeroIndexX = i - 1;
		float zeroIndexY = j - 1;
		if (j%2 == 1){
			posX = unityTileX/2+unityTileX * zeroIndexX;
			posY = unityTileY/2+(unityTileY-0.15f) * zeroIndexY;
		}
		else {
			posX = unityTileX * zeroIndexX;
			posY = unityTileY/2+(unityTileY-0.15f) * zeroIndexY;
		}		
		return new Vector2(posX, posY);
	}
							
	public static Vector2 GetTranslatedCoordinate(float x, float y){
		float i, j;

		j = (int)((((y - unityTileY / 2) / (unityTileY - 0.15f)) + 1) + 0.1f);
		i = (int)((x / unityTileX + 1) + 0.4f);

		return new Vector2(i, j);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
