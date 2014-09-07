using UnityEngine;
using System.Collections.Generic;
using Smooth.Slinq;

public class CharacterManager
{
	public int GetCurrentTileKey()
	{
		return characterMover.GetCurrentTileKey();
	}

	public static CharacterManager CreateInStart(Character characterPrefab, DirectionArrow arrowPrefab)
	{
		return new CharacterManager(characterPrefab, arrowPrefab);
	}

	public static CharacterManager Create(Character characterPrefab, DirectionArrow arrowPrefab)
	{
		return new CharacterManager(characterPrefab, arrowPrefab);
	}

	private CharacterManager(Character characterPrefab, DirectionArrow arrowPrefab)
	{
		this.characterPrefab = characterPrefab;
		this.arrowPrefeb = arrowPrefab;
	}

	public void Init()
	{
		Start();
	}

	private Character characterPrefab;
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
		Battle,
		Waiting,
		DirectionSelected,
        CheckingTileOption
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

	Dictionary<TileManager.TileDirection, Tile> SearchMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderTileDictionary)
	{
		return characterMover.GetTileDictionaryOfMovableTiles(borderTileDictionary);
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

	void MoveAndNotify(Tile toMoveTile)
	{
		var toMoveTileCoord = toMoveTile.GetCoord();
		NetworkManager.SendMoveTile(
				(int)toMoveTileCoord.x,
				(int)toMoveTileCoord.y);

		Move(toMoveTile);
	}

	public void Move(int coordX, int coordY)
	{
		Tile tile = TileManager.GetTileByCoord(coordX, coordY);
		Move(tile);
	}

	public void Move(Tile toMoveTile)
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
		characterInstance = GameObject.Instantiate(characterPrefab) as Character;
	}

	public void InitializeCharacter()
	{
		Tile startTile = TileManager.GetStartTile();
		characterInstance.SetStartTile(startTile);

		Vector3 spawnTilePosition = characterInstance.GetSpawnTile().gameObject.transform.position;
		Vector3 spawnPositionOfCharacter = new Vector3(spawnTilePosition.x, spawnTilePosition.y, Unit.Depth);

		characterInstance.transform.position = spawnPositionOfCharacter;
		Vector2 characterCoordinate = FieldTileUtility.GetCoordFromPosition(spawnPositionOfCharacter.x, spawnPositionOfCharacter.y);

		CharacterMover mover = characterInstance.GetComponent<CharacterMover>();
		mover.InitializeTileKey((int)(characterCoordinate.x * 100 + characterCoordinate.y));

		Camera.main.transform.position = new Vector3(spawnPositionOfCharacter.x, spawnPositionOfCharacter.y, Camera.main.transform.position.z);
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

    void InterectionWithTile()
    {
        int tileKey = GetCurrentTileKey();
        Tile tile = TileManager.GetExistTile(tileKey);
        Tile.TileType tileType = tile.tileType;

        if (tileType == Tile.TileType.Buff)
            characterInstance.SetBuffOrDeBuff();
        else if (tileType == Tile.TileType.Item)
            Debug.Log("Get Item!");
        else if (tileType == Tile.TileType.Jail)
            characterInstance.InJail();
        else if (tileType == Tile.TileType.Save)
            characterInstance.CheckSaveTile(characterMover.GetCurrentTileKey());
        else if (tileType == Tile.TileType.Warp)
            Debug.Log("This Tile is Portal!");
        else
            Debug.Log("Default Tile.");
    }

	Tile toMoveTile = null;

	public void Update ()
	{
		if (moveState != MoveState.Inactive)
		{
			characterInstance.SendMessage("OnCmaeraFollow", characterInstance, SendMessageOptions.DontRequireReceiver);
		}

		if (moveState == MoveState.Moving && UnitUtil.IsEnemyEncounter(GetCurrentTileKey()))
        {
            Debug.LogWarning("Encounter enemy.");
            NetworkManager.StartBattle(UnitUtil.GetEnemyIdOnTile(GetCurrentTileKey()));
            moveState = MoveState.Battle;
        } 
        else if (howManyMove <= 0 && moveState == MoveState.Moving)
        {
            moveState = MoveState.CheckingTileOption;
        } 
        else if (moveState == MoveState.CheckingTileOption)
        {
            moveState = MoveState.Inactive;

            InterectionWithTile();
            
            //FIXME we now always need network.
            if (NetworkManager.isConnected())
            {
                NetworkManager.SendTurnEndMessage();
            } else
            {
                GameManager.gameManagerInstance.PassTurnToNextPlayer();
            }
        }
		else if (moveState == MoveState.Moving)
		{
			var borderDictionary = SearchBorderTiles();
			var movableDictionary = SearchMovableTiles(borderDictionary);

			if (UnitUtil.IsBranch(movableDictionary) == true)
			{
				CreateArrow(movableDictionary);
				moveState = MoveState.Waiting;
			}
			else
			{
				var currentTileKey = GetCurrentTileKey();
				SetDestination(movableDictionary);
				var toMoveTileKey = toMoveTile.GetTileKey();
				MoveAndNotify(toMoveTile);
				howManyMove--;

				var currentTileKeyOfNext = GetCurrentTileKey();

				Debug.LogWarning("TileKeys : " + currentTileKey + ", " + toMoveTileKey + ", " + currentTileKeyOfNext);
				Debug.LogWarning("TileKey enemy? : " +
						UnitUtil.IsEnemyEncounter(currentTileKey) + ", " +
						UnitUtil.IsEnemyEncounter(toMoveTileKey) + ", " +
						UnitUtil.IsEnemyEncounter(currentTileKeyOfNext));
			}
		}
		else if (moveState == MoveState.Battle)
		{
			// Do nothing, wait battle end.
		}
		else if (moveState == MoveState.Waiting)
		{
			// Do nothing, wait user input.
		}
		else if (moveState == MoveState.DirectionSelected)
		{
			Debug.Log("toMoveTile in Update : " + toMoveTile);

			MoveAndNotify(toMoveTile);
			howManyMove--;

			moveState = MoveState.Moving;
		}
	}

	// Called from all users.
	public void BattleLose()
	{
		characterInstance.currentHp = characterInstance.maxHp;
		Move(characterInstance.GetSpawnTile());
		if (moveState == MoveState.Battle && GameManager.gameManagerInstance.isMyCharacterManager(this))
		{
			NetworkManager.SendTurnEndMessage();
		}
		moveState = MoveState.Inactive;
	}

	// Called from all users.
	public void BattleWin()
	{
		if (moveState == MoveState.Battle)
		{
			moveState = MoveState.Moving;
		}
	}
}
