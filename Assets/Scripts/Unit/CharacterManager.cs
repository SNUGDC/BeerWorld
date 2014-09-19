using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Smooth.Slinq;

//For debug in unity ui.
[System.Serializable]
public class CharacterManager
{
	public int GetCurrentTileKey()
	{
		return characterMover.GetCurrentTileKey();
	}

    Character.CharClass charClass = Character.CharClass.Novice;

	public static CharacterManager CreateInStart(Character characterPrefab, DirectionArrow arrowPrefab, Character.CharClass charClass)
	{
		return new CharacterManager(characterPrefab, arrowPrefab, charClass);
	}

    public static CharacterManager Create(Character characterPrefab, DirectionArrow arrowPrefab, Character.CharClass charClass)
	{
		return new CharacterManager(characterPrefab, arrowPrefab, charClass);
	}

    private CharacterManager(Character characterPrefab, DirectionArrow arrowPrefab, Character.CharClass charClass)
	{
		this.characterPrefab = characterPrefab;
		this.arrowPrefeb = arrowPrefab;
        this.charClass = charClass;
	}

	public void Init()
	{
		Start();
	}

	private Character characterPrefab;
	private DirectionArrow arrowPrefeb;
	private Character characterInstance;
	private CharacterMover characterMover;

	private int remainMoveCount = 0;

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
        CheckingTileOption,
        CheckingSaveTile //checking @each frame.
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
		Run.Coroutine(StartTurn());
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

	IEnumerator MoveAndNotify(Tile toMoveTile)
	{
		var toMoveTileCoord = toMoveTile.GetCoord();
		NetworkManager.SendMoveTile(
				(int)toMoveTileCoord.x,
				(int)toMoveTileCoord.y);

		return Move(toMoveTile);
	}

	public IEnumerator Move(int coordX, int coordY)
	{
		Tile tile = TileManager.GetTileByCoord(coordX, coordY);
		return Move(tile);
	}

	public IEnumerator Move(Tile toMoveTile)
	{
		return characterMover.MoveTo(toMoveTile);
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
        characterInstance.SetCharClass(this.charClass);
        Debug.Log("Create : " + charClass);

		Tile startTile = TileManager.GetStartTile();
		characterInstance.SetStartTile(startTile);
		characterInstance.SetIsMine(GameManager.gameManagerInstance.isMyCharacterManager(this));
		characterInstance.SetPlayerId(GameManager.GetNetworkViewID(this));

		characterInstance.Initialize();

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

		//if (Network.isClient == false)
		//{
			//moveState = MoveState.Idle;
			//Debug.Log("MoveState of server : Idle");
			//Run.Coroutine(StartTurn());
		//}
	}

	public void SetMovement(int toMove)
	{
		moveState = MoveState.Moving;
		remainMoveCount = toMove;
	}

    Character.Item SelectRandomItem()
    {
        int random = Random.Range(1, 6); //There are 6 kind of items.

        switch (random)
        {
            case 1:
                return Character.Item.DiceChange;
            case 2:
                return Character.Item.DiceResultChange;
            case 3:
                return Character.Item.Dodge;
            case 4:
                return Character.Item.Berserk;
            case 5:
                return Character.Item.Block;
            case 6:
                return Character.Item.Adding;
            default:
                return Character.Item.None;
        }
    }

    void InterectionWithTile()
    {
        int tileKey = GetCurrentTileKey();
        Tile tile = TileManager.GetExistTile(tileKey);
        Tile.TileType tileType = tile.tileType;

        if (moveState == MoveState.CheckingSaveTile)
        {
            if (tileType == Tile.TileType.Save)
            {
                characterInstance.CheckSaveTile(characterMover.GetCurrentTileKey());
            }
        } 
        else
        {
            if (tileType == Tile.TileType.Buff)
            {
                characterInstance.SetBuffOrDeBuff();
            }
            else if (tileType == Tile.TileType.Item)
            {   
                if (characterInstance.GetNumberOfItems() < Character.MaxInventorySize)
                {
                    Character.Item newItem = SelectRandomItem();
                    characterInstance.GetItem(newItem);
                    Debug.Log("Get Item!");
                }
                else
                {
                    Debug.Log("Inventory is full...");
                }
            }
            else if (tileType == Tile.TileType.Jail)
            {   
                characterInstance.InJail();
            }
            else if (tileType == Tile.TileType.Warp)
            {
                //FIXME : Add warp code!
                //warp tiles are NOT used.
                Debug.Log("This Tile is Portal!");
            }
            else
            {
//                Debug.Log("Default Tile.");
            }
        }
    }

    void UpdateRemainBuffTime()
    {
        characterInstance.UpdateRemainBuffTime();
    }

	Tile toMoveTile = null;

	IEnumerator StartTurn()
	{
		while (moveState != MoveState.Inactive)
		{
			var stateUpdate = Run.Coroutine(StateUpdate());
			yield return stateUpdate.WaitFor;
		}
	}

	public IEnumerator StateUpdate ()
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
        else if (remainMoveCount <= 0 && moveState == MoveState.Moving)
        {
            moveState = MoveState.CheckingTileOption;
        } 
        else if (moveState == MoveState.CheckingTileOption)
        {
            moveState = MoveState.Inactive;

            InterectionWithTile();
            UpdateRemainBuffTime();
						NetworkManager.SendTurnEndMessage();
        } 

        else if (moveState == MoveState.CheckingSaveTile)
        {
            InterectionWithTile();

            moveState = MoveState.Moving;
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

				var moveAndNotify = Run.Coroutine(MoveAndNotify(toMoveTile));
				yield return moveAndNotify.WaitFor;
				remainMoveCount--;

				var currentTileKeyOfNext = GetCurrentTileKey();

                moveState = MoveState.CheckingSaveTile;
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
 			var moveAndNotify = Run.Coroutine(MoveAndNotify(toMoveTile));
			yield return moveAndNotify.WaitFor;
			remainMoveCount--;

			moveState = MoveState.Moving;
		}
	}

	// Called from all users.
	public void BattleLose()
	{
		characterInstance.currentHp = characterInstance.maxHp;
		var move = Run.Coroutine(Move(characterInstance.GetSpawnTile()));
		Debug.LogWarning(moveState.ToString());

		move.ExecuteWhenDone(() => {
			if (moveState == MoveState.Battle && GameManager.gameManagerInstance.isMyCharacterManager(this))
			{
				NetworkManager.SendTurnEndMessage();
			}
			moveState = MoveState.Inactive;
		});
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
