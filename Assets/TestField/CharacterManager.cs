﻿using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	public Character characterPrefeb;
    private Character characterInstance;

    public static CharacterManager characterManagerInstance = null; 

    public int howManyMove = 0;

    public Dictionary<TileManager.TileDirection, Tile> borderDictionary = new Dictionary<TileManager.TileDirection, Tile>();

    void Awake()
    {
        characterManagerInstance = this;
    }

    void SearchBorderTiles () 
    {
        Vector3 position = characterInstance.transform.position;
        Vector2 characterCoordinate = FieldTileUtility.GetTranslatedCoordinate(position.x, position.y);
        Debug.Log("characterCoord : (" + characterCoordinate.x + ", " + characterCoordinate.y + ")");
        borderDictionary = TileManager.GetTileDictionaryOfBorderTiles(characterCoordinate);
    }

    void MoveCharacter () {
        Debug.Log("------Searching------");
        SearchBorderTiles();
        Debug.Log("------Moving------");
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in borderDictionary)
        {
            TileManager.TileDirection direction = pair.Key;
            Tile toMoveTile = pair.Value;
            if (toMoveTile == null)
            {
                continue;
            }

            Vector2 nextTilePosition = new Vector2(toMoveTile.transform.position.x, toMoveTile.transform.position.y);
            Vector2 nextTileCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);
            int nextTileKey = (int)(nextTileCoordinate.x * 100 + nextTileCoordinate.y);

            if ((int)nextTileKey == characterInstance.preTileKey)
            {
                continue;
            }

            characterInstance.preTileKey = characterInstance.currentTileKey;

            characterInstance.transform.position = new Vector3(nextTilePosition.x, nextTilePosition.y, Character.Depth);
            Vector2 newCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);
            characterInstance.currentTileKey = (int)(newCoordinate.x*100 + newCoordinate.y);

            Camera.main.transform.position = new Vector3(characterInstance.transform.position.x, characterInstance.transform.position.y, Camera.main.transform.position.z);

            Debug.Log("key : " + characterInstance.currentTileKey + ", preKey : " + characterInstance.preTileKey);
            Debug.Log("Move to (" + characterInstance.currentTileKey + ")");

            break; // move end.

        }
    }

	// Use this for initialization
	void Start () {
		Tile startTile = TileManager.GetStartTile ();
        Vector3 startTilePosition = startTile.gameObject.transform.position;
        Vector3 startPositionOfCharacter = new Vector3(startTilePosition.x, startTilePosition.y, Character.Depth);
        characterInstance = Instantiate(characterPrefeb, startPositionOfCharacter, Quaternion.identity) as Character; 
        Vector2 characterCoordinate = FieldTileUtility.GetTranslatedCoordinate(startPositionOfCharacter.x, startPositionOfCharacter.y);
        characterInstance.currentTileKey = (int)(characterCoordinate.x * 100 + characterCoordinate.y);
        characterInstance.preTileKey = 000;

        Camera.main.transform.position = new Vector3(startPositionOfCharacter.x, startPositionOfCharacter.y, Camera.main.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        while (howManyMove > 0)
        {
            MoveCharacter ();
            howManyMove--;
        }
	}
}