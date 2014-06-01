using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	public Character characterPrefeb;
    private Character characterInstance;

    public DirectionArrow arrowPrefeb;

    public static CharacterManager characterManagerInstance = null; 

    public int howManyMove = 0;

    public Dictionary<TileManager.TileDirection, Tile> borderDictionary = new Dictionary<TileManager.TileDirection, Tile>();
    public Dictionary<TileManager.TileDirection, Tile> movableDictionary = new Dictionary<TileManager.TileDirection, Tile>();

    public List<GameObject> directionArrowList = new List<GameObject>();

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

    Dictionary<TileManager.TileDirection, Tile> GetTileDictionaryOfMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderDictionary)
    {
        Dictionary<TileManager.TileDirection, Tile> movableDictionary = new Dictionary<TileManager.TileDirection, Tile>();

        TileManager.TileDirection tempKey;
        Tile tempTile;

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in borderDictionary)
        {
            tempKey = pair.Key;
            tempTile = pair.Value;
                        
            if (tempTile == null)
            {
                continue;
            }
            
            if (IsPreTile(tempTile) == true)
            {
                continue;
            }
        
            movableDictionary.Add(tempKey, tempTile);
        }

        return movableDictionary;
    }

    void SearchMovableTiles()
    {
        movableDictionary = GetTileDictionaryOfMovableTiles(borderDictionary);
    }

    bool IsBranch()
    {
        int numberOfMovableDirection = 0;
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            numberOfMovableDirection++;
        }

        if (numberOfMovableDirection > 1)
        {
            Debug.Log("is branch");
            return true;
        }
        
        if (numberOfMovableDirection == 0)
        {
            Debug.Log("There is no movable tile!");
        }
        
        Debug.Log("isn't branch");
        return false;
    }

    bool IsPreTile(Tile tile)
    {
        int preTileKeyOfCharacter = characterInstance.preTileKey;
        int tileKeyOfBorderTile = FieldTileUtility.GetTranslatedTileToKey(tile);

        return preTileKeyOfCharacter == tileKeyOfBorderTile;
    }

    void SelectDirection()
    {
        //TileManager.TileDirection seletedDirection;
        InstantiateArrows();
    }

    void InstantiateArrows ()
    {
        directionArrowList = new List<GameObject>();

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            Vector3 characterPosition = characterInstance.transform.position;
            Vector2 arrowCoordinate = FieldTileUtility.GetTranslatedKeyToCoordinate (pair.Key, characterPosition);
            Debug.Log("Arrow Coordinate : " + arrowCoordinate.x + ", " + arrowCoordinate.y);
            Vector2 arrowPosition = FieldTileUtility.GetTranslatedPosition(arrowCoordinate.x, arrowCoordinate.y);
            Vector3 arrowPositionWithZ = new Vector3 (arrowPosition.x, arrowPosition.y, characterPosition.z);

            GameObject directionArrow = null;
            directionArrow = Instantiate(arrowPrefeb, arrowPositionWithZ, Quaternion.identity) as GameObject;
            DirectionArrow.SetArrowDirection(pair.Key);

            directionArrowList.Add(directionArrow);
        }
    }

    void DestroyAllDirectionArrows()
    {

    }

    void MoveCharacter () {
        Debug.Log("------Searching------");
        SearchBorderTiles();
        SearchMovableTiles();

        //IsBranch();

        if (IsBranch() == true)
        {
            SelectDirection();
        }

        Debug.Log("------Moving------");
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            TileManager.TileDirection direction = pair.Key;
            Tile toMoveTile = pair.Value;
            if (toMoveTile == null)
            {
                continue;
            }

            Vector2 nextTilePosition = new Vector2(toMoveTile.transform.position.x, toMoveTile.transform.position.y);
            Vector2 nextTileCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);

            characterInstance.preTileKey = characterInstance.currentTileKey;

            characterInstance.transform.position = new Vector3(nextTilePosition.x, nextTilePosition.y, Character.Depth);
            Vector2 newCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);
            characterInstance.currentTileKey = FieldTileUtility.GetTranslatedCoordinateToKey(newCoordinate);
            
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
