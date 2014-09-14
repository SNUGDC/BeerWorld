using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager
{
	public int GetCurrentTileKey()
    {
        return characterMover.GetCurrentTileKey();
    }

	private Tile spawnTile = null;

	public string enemyId
	{
		get;
		private set;
	}

	public static EnemyManager CreateInStart(Unit unitPrefab, string enemyId)
	{
		Tile startTile = TileManager.GetStartTile ();
		return new EnemyManager(unitPrefab, startTile, enemyId);
	}

	public static EnemyManager Create(Unit unitPrefab, Tile spawnTile, string enemyId)
	{
		return new EnemyManager(unitPrefab, spawnTile, enemyId);
	}

	private EnemyManager(Unit unitPrefab, Tile spawnTile, string enemyId)
	{
		this.unitPrefab = unitPrefab;
		this.spawnTile = spawnTile;
		this.enemyId = enemyId;
	}

	public void Init()
	{
		Start();
	}

	private Unit unitPrefab;
	private Unit unitInstance;
	private CharacterMover characterMover;

	private int remainMoveCount = 0;

	public Unit GetUnitInstance()
	{
		return unitInstance;
	}

	public enum MoveState
	{
		Inactive, // other user's turn.
		Idle,  // diceRoller btn visible.
		Moving,
		Battle
	}

	[SerializeField]
	private MoveState moveState = MoveState.Inactive;

	public EnemyManager.MoveState GetMoveState()
	{
		return moveState;
	}

	public void ChangeMoveStateToIdle()
	{
		moveState = MoveState.Idle;
		SetMovement(1);
		Run.Coroutine(StartTurn());
	}

	Dictionary<TileManager.TileDirection, Tile> SearchBorderTiles ()
	{
		Vector3 position = unitInstance.transform.position;
		Vector2 unitCoordinate = FieldTileUtility.GetCoordFromPosition(position.x, position.y);
		return TileManager.GetTileDictionaryOfBorderTiles(unitCoordinate);
	}

	Dictionary<TileManager.TileDirection, Tile> SearchMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderTileDictionary)
	{
		return characterMover.GetTileDictionaryOfMovableTiles(borderTileDictionary);
	}

	IEnumerator MoveAndNotify(Tile toMoveTile)
	{
		NetworkManager.MoveEnemy(toMoveTile.GetTileKey(), enemyId);
		return Move(toMoveTile);
	}

	public IEnumerator Move(int coordX, int coordY)
	{
		Tile tile = TileManager.GetTileByCoord(coordX, coordY);
		return Move(tile);
	}

	public IEnumerator Move(int tileKey)
	{
		Tile tile = TileManager.GetExistTile(tileKey);
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

	void InstantiateUnit()
	{
		unitInstance = GameObject.Instantiate(unitPrefab) as Unit;
	}

	public void InitializeUnit()
	{
		Vector3 spawnTilePosition = spawnTile.gameObject.transform.position;
		Vector3 spawnPositionOfUnit = new Vector3(spawnTilePosition.x, spawnTilePosition.y, Unit.Depth);

		unitInstance.transform.position = spawnPositionOfUnit;
		Vector2 unitCoordinate = FieldTileUtility.GetCoordFromPosition(spawnPositionOfUnit.x, spawnPositionOfUnit.y);

		CharacterMover mover = unitInstance.GetComponent<CharacterMover>();
		mover.InitializeTileKey((int)(unitCoordinate.x * 100 + unitCoordinate.y));

		Camera.main.transform.position = new Vector3(spawnPositionOfUnit.x, spawnPositionOfUnit.y, Camera.main.transform.position.z);
	}

	// Use this for initialization
	void Start () {
		InstantiateUnit();
		InitializeUnit();
		characterMover = unitInstance.GetComponent<CharacterMover>();
	}

	public void SetMovement(int toMove)
	{
		moveState = MoveState.Moving;
		remainMoveCount = toMove;
	}

	Tile toMoveTile = null;

	IEnumerator StartTurn()
	{
		// check Enemy is deleted.
		while (unitInstance != null && moveState != MoveState.Inactive)
		{
			var stateUpdate = Run.Coroutine(StateUpdate());
			yield return stateUpdate.WaitFor;
		}
	}

	public IEnumerator StateUpdate ()
	{
		if (moveState != MoveState.Inactive)
		{
			unitInstance.SendMessage("OnCmaeraFollow", unitInstance, SendMessageOptions.DontRequireReceiver);
		}

		if (moveState == MoveState.Moving && UnitUtil.IsPlayerEncounter(GetCurrentTileKey()))
		{
			// FIXME: Battle with only room owner.
			var playerId = UnitUtil.GetPlayerIdOnTile(GetCurrentTileKey());
			NetworkManager.StartBattleFromEnemy(enemyId, playerId);
			moveState = MoveState.Battle;
		}

		if (remainMoveCount <= 0 && moveState == MoveState.Moving)
		{
			moveState = MoveState.Inactive;
			GameManager.gameManagerInstance.PassTurnToNextPlayer();
		}
		else if (moveState == MoveState.Moving)
		{
			var borderDictionary = SearchBorderTiles();
			var movableDictionary = SearchMovableTiles(borderDictionary);

			SetDestination(movableDictionary);

			var moveAndNotify = Run.Coroutine(MoveAndNotify(toMoveTile));
			yield return moveAndNotify.WaitFor;

			remainMoveCount--;
		}
	}

	public void Kill()
	{
		GameObject.Destroy(unitInstance.gameObject);
	}

	// Called from all users.
	public void BattleWin()
	{
		moveState = MoveState.Moving;
	}

	// Called from all users.
	public void BattleLose()
	{
		if (moveState == MoveState.Battle)
		{
			GameManager.gameManagerInstance.PassTurnToNextPlayer();
		}
		GameManager.gameManagerInstance.KillEnemy(enemyId);
	}
}
