using UnityEngine;
using System.Collections.Generic;

public class EnemyManager
{
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

	private int howManyMove = 0;

	public Unit GetUnitInstance()
	{
		return unitInstance;
	}

	public enum MoveState
	{
		Inactive, // other user's turn.
		Idle,  // diceRoller btn visible.
		Moving
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

	void MoveAndNotify(Tile toMoveTile)
	{
		Move(toMoveTile);
		NetworkManager.MoveEnemy(toMoveTile.GetTileKey(), enemyId);
	}

	public void Move(int coordX, int coordY)
	{
		Tile tile = TileManager.GetTileByCoord(coordX, coordY);
		Move(tile);
	}

	public void Move(int tileKey)
	{
		Tile tile = TileManager.GetExistTile(tileKey);
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
		howManyMove = toMove;
	}

	Tile toMoveTile = null;

	public void Update ()
	{
		if (moveState != MoveState.Inactive)
		{
			unitInstance.SendMessage("OnCmaeraFollow", unitInstance, SendMessageOptions.DontRequireReceiver);
		}

		if (howManyMove <= 0 && moveState == MoveState.Moving)
		{
			moveState = MoveState.Inactive;
			GameManager.gameManagerInstance.PassTurnToNextPlayer();
			return;
		}

		if (moveState == MoveState.Moving)
		{
			var borderDictionary = SearchBorderTiles();
			var movableDictionary = SearchMovableTiles(borderDictionary);

			SetDestination(movableDictionary);
			MoveAndNotify(toMoveTile);
			howManyMove--;
		}
	}
}
