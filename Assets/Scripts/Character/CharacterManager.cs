﻿using UnityEngine;
using System.Collections.Generic;

public class CharacterManager
{
    public CharacterManager(Character characterPrefeb, DirectionArrow arrowPrefab)
    {
        this.characterPrefeb = characterPrefeb;
        this.arrowPrefeb = arrowPrefab;
    }

    public void Init()
    {
        Start();
    }

	private Character characterPrefeb;
    private DirectionArrow arrowPrefeb;
    private Character characterInstance;
    private CharacterMover characterMover;

    private int howManyMove = 0;

    public Character GetCharacterInstance()
    {
        return characterInstance;
    }

    public enum MoveState
    {
        Inactive, // other user's turn.
        Idle,  // diceRoller btn visible.
        Moving,
        Waiting,
        DirectionSelected
    }
    [SerializeField]
    private MoveState moveState = MoveState.Inactive;

    public CharacterManager.MoveState GetMoveState()
    {
        return moveState;
    }

    public void ChangeMoveStateToIdle()
    {
        moveState = MoveState.Idle;
        Debug.Log("Changed MoveState to Idle @" + NetworkManager.networkInstance.GetNetworkID());
    }

    public List<DirectionArrow> directionArrowList = new List<DirectionArrow>();

    Dictionary<TileManager.TileDirection, Tile> SearchBorderTiles () 
    {
        Vector3 position = characterInstance.transform.position;
        Vector2 characterCoordinate = FieldTileUtility.GetCoordFromPosition(position.x, position.y);
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

            if (tempTile.tileType == Tile.TileType.Start)
            {
                continue;
            }
        
            movableDictionary.Add(tempKey, tempTile);
        }

        return movableDictionary;
    }

    Dictionary<TileManager.TileDirection, Tile> SearchMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderTileDictionary)
    {
        return GetTileDictionaryOfMovableTiles(borderTileDictionary);
    }

    bool IsBranch(Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        int numberOfMovableDirection = 0;
        numberOfMovableDirection = movableDictionary.Count;

        if (numberOfMovableDirection > 1)
        {
            return true;
        }
        
        if (numberOfMovableDirection == 0)
        {
            Debug.Log("There is no movable tile!");
        }

        return false;
    }

    bool IsPreTile(Tile tile)
    {
        int preTileKeyOfCharacter = characterMover.preTileKey;
        int tileKeyOfBorderTile = FieldTileUtility.GetKeyFromTile(tile);

        return preTileKeyOfCharacter == tileKeyOfBorderTile;
    }

    bool IsPrePreTile(Tile tile)
    {
        int prePreTileKeyOfCharacter = characterMover.prePreTileKey;
        int tileKeyOfBorderTile = FieldTileUtility.GetKeyFromTile(tile);

        return prePreTileKeyOfCharacter == tileKeyOfBorderTile;
    }

    void CreateArrow (Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        directionArrowList = new List<DirectionArrow>();

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            TileManager.TileDirection direction = pair.Key;

            Vector3 characterPosition = characterInstance.transform.position;
            Vector2 arrowCoordinate = FieldTileUtility.GetCoordOfDirectionByPosition(direction, characterPosition);
            Vector2 arrowPosition = FieldTileUtility.GetPositionFromCoordinate(arrowCoordinate.x, arrowCoordinate.y);
            Vector3 arrowPositionWithZ = new Vector3 (arrowPosition.x, arrowPosition.y, characterPosition.z);

            DirectionArrow directionArrow = null;
            directionArrow = GameObject.Instantiate(arrowPrefeb, arrowPositionWithZ, Quaternion.identity) as DirectionArrow;
            
            DirectionArrow directionArrowScript = directionArrow.gameObject.GetComponent<DirectionArrow>();
            directionArrowScript.SetArrowDirection(direction);

            directionArrowList.Add(directionArrow);
        }
    }

    public void DestroyAllDirectionArrows()
    {
        moveState = MoveState.DirectionSelected;

        foreach(DirectionArrow arrow in directionArrowList)
        {
            GameObject.Destroy(arrow.gameObject);
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
        characterMover.MoveTo(toMoveTile);
    }

    void SetDestination (Dictionary<TileManager.TileDirection, Tile> movableDictionary)
    {
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionary)
        {
            toMoveTile = pair.Value;
            if (toMoveTile == null)
            {
                continue;
            }
        }
    }

    public void SetDestinationByArrow(TileManager.TileDirection direction)
    {        
        var borderDictionary = SearchBorderTiles();
        var movableDictionary = SearchMovableTiles(borderDictionary);

        toMoveTile = movableDictionary[direction];  
    }

    void InstantiateCharacter()
    {
        characterInstance = GameObject.Instantiate(characterPrefeb) as Character; 
    }

    public void InitializeCharacter()
    {
		Tile startTile = TileManager.GetStartTile ();
        Vector3 startTilePosition = startTile.gameObject.transform.position;
        Vector3 startPositionOfCharacter = new Vector3(startTilePosition.x, startTilePosition.y, Character.Depth);

        characterInstance.transform.position = startPositionOfCharacter;
        Vector2 characterCoordinate = FieldTileUtility.GetCoordFromPosition(startPositionOfCharacter.x, startPositionOfCharacter.y);

				CharacterMover mover = characterInstance.GetComponent<CharacterMover>();
				mover.InitializeTileKey((int)(characterCoordinate.x * 100 + characterCoordinate.y));

        Camera.main.transform.position = new Vector3(startPositionOfCharacter.x, startPositionOfCharacter.y, Camera.main.transform.position.z);
    }

	// Use this for initialization
	void Start () {
        InstantiateCharacter();
        InitializeCharacter();
        characterMover = characterInstance.GetComponent<CharacterMover>();
        
        if (Network.isClient == false)
        {
            moveState = MoveState.Idle;
            Debug.Log("MoveState of server : Idle");
        }
	}

    public void SetMovement(int toMove)
    {
        moveState = MoveState.Moving;
        howManyMove = toMove;
    }
	
    Tile toMoveTile = null;

    void cameraFollow()
    {
        Camera.main.transform.position = new Vector3(
                characterInstance.transform.position.x, 
                characterInstance.transform.position.y, 
                Camera.main.transform.position.z);
    }

	// Update is called once per frame
	public void Update ()
    {
        if (moveState != MoveState.Inactive)
        {
            cameraFollow();
        }

        if (howManyMove <= 0 && moveState == MoveState.Moving)
        {
					if (NetworkManager.isConnected())
					{
            moveState = MoveState.Inactive;
					}
					else
					{
						// For local test.
            moveState = MoveState.Idle;
					}

            NetworkManager.SendTurnEndMessage();
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
