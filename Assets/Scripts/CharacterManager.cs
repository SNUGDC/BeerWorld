using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	public Character characterPrefeb;
    private Character characterInstance;

    public DirectionArrow arrowPrefeb;

    public static CharacterManager characterManagerInstance = null; 

    public int howManyMove = 0;
    private bool isMovable = false;
    public bool isSelectDirection = false;

    //public Dictionary<TileManager.TileDirection, Tile> borderDictionary = new Dictionary<TileManager.TileDirection, Tile>();
    //public Dictionary<TileManager.TileDirection, Tile> movableDictionary = new Dictionary<TileManager.TileDirection, Tile>();

    public List<DirectionArrow> directionArrowList = new List<DirectionArrow>();

    void Awake()
    {
        characterManagerInstance = this;
    }

    Dictionary<TileManager.TileDirection, Tile> SearchBorderTiles () 
    {
        Vector3 position = characterInstance.transform.position;
        Vector2 characterCoordinate = FieldTileUtility.GetTranslatedCoordinate(position.x, position.y);
        Debug.Log("characterCoord : (" + characterCoordinate.x + ", " + characterCoordinate.y + ")");
        return TileManager.GetTileDictionaryOfBorderTiles(characterCoordinate);
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

    Dictionary<TileManager.TileDirection, Tile>  SearchMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderTileDictionary)
    {
        return GetTileDictionaryOfMovableTiles(borderTileDictionary);
    }

    bool IsBranch(Dictionary<TileManager.TileDirection, Tile> movableDictionary)
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

    void CreateArrow (Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        directionArrowList = new List<DirectionArrow>();

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            Vector3 characterPosition = characterInstance.transform.position;
            Vector2 arrowCoordinate = FieldTileUtility.GetTranslatedKeyToCoordinate (pair.Key, characterPosition);
            Debug.Log("Arrow Coordinate : " + arrowCoordinate.x + ", " + arrowCoordinate.y);
            Vector2 arrowPosition = FieldTileUtility.GetTranslatedPosition(arrowCoordinate.x, arrowCoordinate.y);
            Vector3 arrowPositionWithZ = new Vector3 (arrowPosition.x, arrowPosition.y, characterPosition.z);

            Debug.Log("Arrow Key : " + pair.Key);

            DirectionArrow directionArrow = null;
            directionArrow = Instantiate(arrowPrefeb, arrowPositionWithZ, Quaternion.identity) as DirectionArrow;
            
            DirectionArrow directionArrowScript = directionArrow.gameObject.GetComponent<DirectionArrow>();
            directionArrowScript.SetArrowDirection(pair.Key);

            directionArrowList.Add(directionArrow);
        }
    }

    public void DestroyAllDirectionArrows()
    {
        var borderDictionary = SearchBorderTiles();
        var movableDictionary = SearchMovableTiles(borderDictionary);

        foreach(DirectionArrow arrow in directionArrowList)
        {
            Destroy(arrow.gameObject);
        }
        directionArrowList = new List<DirectionArrow>();

        isMovable = true;
        isSelectDirection = false;
        toMoveTile = SetDestination(movableDictionary);
    }

    void MoveCharacter(Tile toMoveTile)
    {
        Debug.Log("------Moving------");

        Vector2 nextTilePosition = new Vector2(toMoveTile.transform.position.x, toMoveTile.transform.position.y);
        Vector2 nextTileCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);

        characterInstance.preTileKey = characterInstance.currentTileKey;

        characterInstance.transform.position = new Vector3(nextTilePosition.x, nextTilePosition.y, Character.Depth);
        Vector2 newCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);
        characterInstance.currentTileKey = FieldTileUtility.GetTranslatedCoordinateToKey(newCoordinate);
        
        Camera.main.transform.position = new Vector3(characterInstance.transform.position.x, characterInstance.transform.position.y, Camera.main.transform.position.z);

        Debug.Log("key : " + characterInstance.currentTileKey + ", preKey : " + characterInstance.preTileKey);
        Debug.Log("Move to (" + characterInstance.currentTileKey + ")");
    }

    Tile SetDestination (Dictionary<TileManager.TileDirection, Tile> movableDictionary) {
        Tile toMoveTile = null;

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            TileManager.TileDirection direction = pair.Key;
            toMoveTile = pair.Value;
            if (toMoveTile == null)
            {
                continue;
            }
        }

        return toMoveTile;
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

    bool IsArrowCreated()
    {
        return isSelectDirection;
    }

    void CreateArrowIfBranch(Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        if (IsBranch(movableDictionary) == true && IsArrowCreated() == false)
        {
            CreateArrow(movableDictionary);
            isSelectDirection = true;
        }
    }
	
    Tile toMoveTile = null;

	// Update is called once per frame
	void Update ()
    {
        if (howManyMove <= 0)
        {
            return;
        }

        Debug.Log("------Searching------");
        var borderDictionary = SearchBorderTiles();
        var movableDictionary = SearchMovableTiles(borderDictionary);
        CreateArrowIfBranch(movableDictionary);


        if (IsBranch(movableDictionary) == false)
        {            
            isMovable = true;
            toMoveTile = SetDestination(movableDictionary);
        }
        if (isMovable == true)
        {
            MoveCharacter (toMoveTile);
            howManyMove--;
            isMovable = false;
        }
        
	}
}
