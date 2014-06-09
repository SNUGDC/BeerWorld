using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	public Character characterPrefeb;
    private Character characterInstance;

    public DirectionArrow arrowPrefeb;

    public static CharacterManager characterManagerInstance = null; 

    private int howManyMove = 0;

    enum MoveState
    {
        Idle,
        Moving,
        Waiting,
        DirectionSelected
    }
    private MoveState moveState = MoveState.Idle;

    public List<DirectionArrow> directionArrowList = new List<DirectionArrow>();

    void Awake()
    {
        characterManagerInstance = this;
    }

    Dictionary<TileManager.TileDirection, Tile> SearchBorderTiles () 
    {
        Vector3 position = characterInstance.transform.position;
        Vector2 characterCoordinate = FieldTileUtility.GetTranslatedCoordinate(position.x, position.y);
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

            if (IsPrePreTile(tempTile) == true)
            {
                continue;
            }

            /*if (tempTile.TileType == Start)
            {
                continue;
            }*/
        
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
        int tileKeyOfBorderTile = FieldTileUtility.GetKeyFromTile(tile);

        return preTileKeyOfCharacter == tileKeyOfBorderTile;
    }

    bool IsPrePreTile(Tile tile)
    {
        int prePreTileKeyOfCharacter = characterInstance.prePreTileKey;
        int tileKeyOfBorderTile = FieldTileUtility.GetKeyFromTile(tile);

        return prePreTileKeyOfCharacter == tileKeyOfBorderTile;
    }

    void CreateArrow (Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        directionArrowList = new List<DirectionArrow>();

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            Vector3 characterPosition = characterInstance.transform.position;
            Vector2 arrowCoordinate = FieldTileUtility.GetTranslatedKeyToCoordinate (pair.Key, characterPosition);
            Vector2 arrowPosition = FieldTileUtility.GetTranslatedPosition(arrowCoordinate.x, arrowCoordinate.y);
            Vector3 arrowPositionWithZ = new Vector3 (arrowPosition.x, arrowPosition.y, characterPosition.z);

            DirectionArrow directionArrow = null;
            directionArrow = Instantiate(arrowPrefeb, arrowPositionWithZ, Quaternion.identity) as DirectionArrow;
            
            DirectionArrow directionArrowScript = directionArrow.gameObject.GetComponent<DirectionArrow>();
            directionArrowScript.SetArrowDirection(pair.Key);

            directionArrowList.Add(directionArrow);
        }
    }

    public void DestroyAllDirectionArrows()
    {
        moveState = MoveState.DirectionSelected;

        foreach(DirectionArrow arrow in directionArrowList)
        {
            Destroy(arrow.gameObject);
        }
        directionArrowList = new List<DirectionArrow>();
    }

    void MoveCharacterAndNotify(Tile toMoveTile)
    {
        var toMoveTileCoord = toMoveTile.GetCoord();
        NetworkManager.SendMoveTile(
                (int)toMoveTileCoord.x,
                (int)toMoveTileCoord.y);

        MoveCharacter(toMoveTile);
    }

    public void MoveCharacter(int coordX, int coordY)
    {
        Tile tile = TileManager.GetTileByCoord(coordX, coordY);
        MoveCharacter(tile);
    }

    public void MoveCharacter(Tile toMoveTile)
    {
        //Debug.Log("------Moving------");

        Vector2 nextTilePosition = new Vector2(toMoveTile.transform.position.x, toMoveTile.transform.position.y);
        Vector2 nextTileCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);

        characterInstance.prePreTileKey = characterInstance.preTileKey;
        characterInstance.preTileKey = characterInstance.currentTileKey;

        characterInstance.transform.position = new Vector3(nextTilePosition.x, nextTilePosition.y, Character.Depth);
        Vector2 newCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextTilePosition.x, nextTilePosition.y);
        characterInstance.currentTileKey = FieldTileUtility.GetKeyFromCoord(newCoordinate);
        
        Camera.main.transform.position = new Vector3(characterInstance.transform.position.x, characterInstance.transform.position.y, Camera.main.transform.position.z);

        //Debug.Log("key : " + characterInstance.currentTileKey + ", preKey : " + characterInstance.preTileKey);
        //Debug.Log("Move to (" + characterInstance.currentTileKey + ")");
    }

    void SetDestination (Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            TileManager.TileDirection direction = pair.Key;
            toMoveTile = pair.Value;
            if (toMoveTile == null)
            {
                continue;
            }
        }
    }

    public void SetDestinationByArrow(TileManager.TileDirection tileKey)
    {        
        var borderDictionary = SearchBorderTiles();
        var movableDictionary = SearchMovableTiles(borderDictionary);

        toMoveTile = movableDictionary[tileKey];  
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
        characterInstance.prePreTileKey = 000;

        Camera.main.transform.position = new Vector3(startPositionOfCharacter.x, startPositionOfCharacter.y, Camera.main.transform.position.z);
	}

    public void SetMovement(int toMove)
    {
        moveState = MoveState.Moving;
        howManyMove = toMove;
    }
	
    Tile toMoveTile = null;

	// Update is called once per frame
	void Update ()
    {
        if (howManyMove <= 0)
        {
            moveState = MoveState.Idle;
            return;
        }

        if (moveState == MoveState.Moving)
        {
            var borderDictionary = SearchBorderTiles();
            var movableDictionary = SearchMovableTiles(borderDictionary);
            
            if (IsBranch(movableDictionary) == true)
            {
                CreateArrow(movableDictionary);
                moveState = MoveState.Waiting;
            }
            else
            {
                SetDestination(movableDictionary);
                MoveCharacterAndNotify(toMoveTile);
                howManyMove--;
            }
        }
        else if (moveState == MoveState.Waiting)
        {
            // Do nothing, wait user input.
        }
        else if (moveState == MoveState.DirectionSelected)
        {
            Debug.Log("toMoveTile in Update : " + toMoveTile);

            MoveCharacterAndNotify(toMoveTile);
            howManyMove--;

            moveState = MoveState.Moving;
        }
	}
}
