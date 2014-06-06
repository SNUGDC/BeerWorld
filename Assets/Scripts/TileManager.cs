﻿using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

    public enum TileDirection
    {
        MidRight,
        MidLeft,
        UpLeft,
        DownLeft,
        UpRight,
        DownRight
    }

	private static TileManager singletonInstance = null;

	public static Tile GetStartTile()
	{
		foreach (var tile in singletonInstance.tileList)
        {
            if (tile.IsStartTile())
            {
                return tile;
            }
        }

        Debug.LogError("Cannot find start tile.");
        return null;
	}

    public Tile GetExistTile(int tileKey)
    {
        if (singletonInstance.tileDictionary.ContainsKey(tileKey) == false)
        {
            return null;
        }
        else
        {
            return singletonInstance.tileDictionary[tileKey];
        }
    }

    static int MakeTileKeyFromCoord(float x, float y)
    {
        return (int)(x * 100 + y);
    }

    public static Dictionary<TileDirection, Tile> GetTileDictionaryOfBorderTiles(Vector2 characterCoordinate)
    {
        Dictionary<TileDirection, Tile> borderDictionary = new Dictionary<TileDirection, Tile>();
        if ((int)characterCoordinate.y % 2 == 1) //odd number
        {
            int upLeftKey = MakeTileKeyFromCoord(characterCoordinate.x, characterCoordinate.y + 1);
            int midLeftKey = MakeTileKeyFromCoord(characterCoordinate.x - 1, characterCoordinate.y);
            int downLeftKey = MakeTileKeyFromCoord(characterCoordinate.x, characterCoordinate.y - 1);
            int upRightKey = MakeTileKeyFromCoord(characterCoordinate.x + 1, characterCoordinate.y + 1);
            int midRightKey = MakeTileKeyFromCoord(characterCoordinate.x + 1, characterCoordinate.y);
            int downRightKey = MakeTileKeyFromCoord(characterCoordinate.x + 1, characterCoordinate.y - 1);

            borderDictionary.Add(TileDirection.UpLeft, singletonInstance.GetExistTile(upLeftKey));
            borderDictionary.Add(TileDirection.MidLeft, singletonInstance.GetExistTile(midLeftKey));
            borderDictionary.Add(TileDirection.DownLeft, singletonInstance.GetExistTile(downLeftKey));
            borderDictionary.Add(TileDirection.UpRight, singletonInstance.GetExistTile(upRightKey));
            borderDictionary.Add(TileDirection.MidRight,singletonInstance.GetExistTile(midRightKey));
            borderDictionary.Add(TileDirection.DownRight, singletonInstance.GetExistTile(downRightKey));
        } 
        else //even number
        {
            int upLeftKey = MakeTileKeyFromCoord(characterCoordinate.x - 1, characterCoordinate.y + 1);
            int midLeftKey = MakeTileKeyFromCoord(characterCoordinate.x - 1, characterCoordinate.y);
            int downLeftKey = MakeTileKeyFromCoord(characterCoordinate.x - 1, characterCoordinate.y - 1);
            int upRightKey = MakeTileKeyFromCoord(characterCoordinate.x, characterCoordinate.y + 1);
            int midRightKey = MakeTileKeyFromCoord(characterCoordinate.x + 1, characterCoordinate.y);
            int downRightKey = MakeTileKeyFromCoord(characterCoordinate.x, characterCoordinate.y - 1);
            
            borderDictionary.Add(TileDirection.UpLeft, singletonInstance.GetExistTile(upLeftKey));
            borderDictionary.Add(TileDirection.MidLeft, singletonInstance.GetExistTile(midLeftKey));
            borderDictionary.Add(TileDirection.DownLeft, singletonInstance.GetExistTile(downLeftKey));
            borderDictionary.Add(TileDirection.UpRight, singletonInstance.GetExistTile(upRightKey));
            borderDictionary.Add(TileDirection.MidRight,singletonInstance.GetExistTile(midRightKey));
            borderDictionary.Add(TileDirection.DownRight, singletonInstance.GetExistTile(downRightKey));
        }

        return borderDictionary;
    }

	List<Tile> tileList = new List<Tile>();

    Dictionary<int, Tile> tileDictionary = new Dictionary<int, Tile>();

	void Awake () {

		singletonInstance = this;

        Tile[] tiles = FindObjectsOfType(typeof(Tile)) as Tile[];
        foreach (Tile tile in tiles)
        {
            tileList.Add(tile);
            Vector2 tilePreCoordinate = FieldTileUtility.GetTranslatedCoordinate(tile.transform.localPosition.x, tile.transform.localPosition.y);
			Vector2 tileCoordinate = new Vector2(tilePreCoordinate.x, tilePreCoordinate.y); 
            int tileKey = (int)(tileCoordinate.x * 100 + tileCoordinate.y);
            tile.SetTileKey(tileKey);
            tileDictionary.Add(tileKey, tile);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}