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
    private Enemy.EnemyType enemyType = Enemy.EnemyType.None;

	public string enemyId
	{
		get;
		private set;
	}

	public static EnemyManager CreateInStart(Unit unitPrefab, string enemyId)
	{
		Tile startTile = TileManager.GetStartTile ();
        Enemy.EnemyType type = Enemy.EnemyType.None;
		return new EnemyManager(unitPrefab, startTile, type, enemyId);
	}

	public static EnemyManager Create(Unit unitPrefab, Tile spawnTile, Enemy.EnemyType type, string enemyId)
	{
		return new EnemyManager(unitPrefab, spawnTile, type, enemyId);
	}

	private EnemyManager(Unit unitPrefab, Tile spawnTile, Enemy.EnemyType type, string enemyId)
	{
		this.unitPrefab = unitPrefab;
		this.spawnTile = spawnTile;
        this.enemyType = type;
		this.enemyId = enemyId;
	}

	public void Init()
	{
		Start();
	}

	private Unit unitPrefab;
	private Enemy enemyInstance;
	private CharacterMover characterMover;

	private int remainMoveCount = 0;
    int makeCooldown = 2;

	public Enemy GetEnemyInstance()
	{
		return enemyInstance;
	}

	public enum MoveState
	{
		Inactive, // other user's turn.
		Idle,  // diceRoller btn visible.
		Moving,
		Battle,
        MakingComplete
	}

	[SerializeField]
	private MoveState moveState = MoveState.Inactive;

	public EnemyManager.MoveState GetMoveState()
	{
		return moveState;
	}

    List<BDice.Species> GetMoveDices(Enemy enemy, List<BDice.Species> moveDices)
    {
        for (int i = 0; i < enemy.numberOfMoveDice; i++)
        {
            moveDices.Add(enemy.speciesOfMoveDice);
        }
        return moveDices;
    }

    int CalculateMoveDicesResult()
    {
        List<BDice.Species> moveDices = new List<BDice.Species>();
        moveDices = GetMoveDices(enemyInstance, moveDices);
        int diceResult = 0;
        for (int i = 0; i < moveDices.Count; i++)
        {
            diceResult += Dice.Roll(moveDices [i]);
        }

        return diceResult;
    }

    Tile SelectRandomTile(Dictionary<TileManager.TileDirection, Tile> emptyTiles)
    {
        int size = emptyTiles.Count;
        int randomKey = Random.Range(0, size - 1);

        List<Tile> targetTiles = new List<Tile>();
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in emptyTiles)
        {
            targetTiles.Add(pair.Value);
        }

        Tile selectedTile = targetTiles[randomKey];
        return selectedTile;
    }

    Dictionary<TileManager.TileDirection, Tile> ExceptTileWithUnit(Dictionary<TileManager.TileDirection, Tile> movableTiles)
    {
        Dictionary<TileManager.TileDirection, Tile> emptyTiles = new Dictionary<TileManager.TileDirection, Tile>();

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableTiles)
        {
            int tileKey = pair.Value.GetTileKey();
            if (UnitUtil.IsPlayerEncounter(tileKey) && UnitUtil.IsEnemyEncounter(tileKey))
            {
                emptyTiles.Add(pair.Key, pair.Value);
            }
        }

        return emptyTiles;
    }

    void MakeSmallestEnemy()
    {
        var borderTiles = SearchBorderTiles();
        var movableTiles = SearchMovableTiles(borderTiles);
        //FIXME : CANNOT except tile including character or other smallest enemy.
        var emptyTiles = ExceptTileWithUnit(movableTiles);
        if (emptyTiles.Count == 0)
        {
            Debug.Log("No empty tile");
            return;
        }
        Tile placeEnemyTile = SelectRandomTile(emptyTiles);
        EnemyInfo newEnemyInfo = new EnemyInfo(placeEnemyTile.GetTileKey(), Enemy.EnemyType.Smallest);

        NetworkManager.MakeEnemy(newEnemyInfo);
    }

	public void ChangeMoveStateToIdle()
	{
        Enemy.EnemyType enemyType = enemyInstance.GetEnemyType();
        moveState = MoveState.Idle;

        if (enemyType == Enemy.EnemyType.Smallest)
        {
            int diceResult = CalculateMoveDicesResult();
            SetMovement(diceResult);
        } 
        else if (enemyType == Enemy.EnemyType.Middle)
        {
            if (makeCooldown == 0)
            {
                MakeSmallestEnemy();
                makeCooldown = 2;
            }
            makeCooldown--;
            moveState = MoveState.MakingComplete;
        } 
        else
        {
            //none.
        }
		
        Run.Coroutine(StartTurn());
	}

	Dictionary<TileManager.TileDirection, Tile> SearchBorderTiles ()
	{
		Vector3 position = enemyInstance.transform.position;
		Vector2 unitCoordinate = FieldTileUtility.GetCoordFromPosition(position.x, position.y);
		return TileManager.GetTileDictionaryOfBorderTiles(unitCoordinate);
	}

	Dictionary<TileManager.TileDirection, Tile> SearchMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderTileDictionary)
	{
		return characterMover.GetTileDictionaryOfMovableTiles(borderTileDictionary);
	}

    Dictionary<TileManager.TileDirection, Tile> SearchMovableTilesWithoutSaveTiles(Dictionary<TileManager.TileDirection, Tile> movableTiles)
    {
        Dictionary<TileManager.TileDirection, Tile> movableTilesWithoutSaveTiles = new Dictionary<TileManager.TileDirection, Tile>();

        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableTiles)
        {
            if (pair.Value.tileType != Tile.TileType.Save)
            {
                movableTilesWithoutSaveTiles.Add(pair.Key, pair.Value);
            }
        }

        return movableTilesWithoutSaveTiles;
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

	void SetDestination (Dictionary<TileManager.TileDirection, Tile> movableDictionaryWithoutSaveTiles)
	{
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in movableDictionaryWithoutSaveTiles)
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
		enemyInstance = GameObject.Instantiate(unitPrefab) as Enemy;
	}

	public void InitializeUnit()
	{
        enemyInstance.SetEnemyType(this.enemyType);

        Vector3 spawnTilePosition = spawnTile.gameObject.transform.position;
		Vector3 spawnPositionOfUnit = new Vector3(spawnTilePosition.x, spawnTilePosition.y, Unit.Depth);

		enemyInstance.transform.position = spawnPositionOfUnit;
		Vector2 unitCoordinate = FieldTileUtility.GetCoordFromPosition(spawnPositionOfUnit.x, spawnPositionOfUnit.y);

		CharacterMover mover = enemyInstance.GetComponent<CharacterMover>();
		mover.InitializeTileKey((int)(unitCoordinate.x * 100 + unitCoordinate.y));

		//Camera.main.transform.position = new Vector3(spawnPositionOfUnit.x, spawnPositionOfUnit.y, Camera.main.transform.position.z);
	}

	// Use this for initialization
	void Start () {
		InstantiateUnit();
		InitializeUnit();
		characterMover = enemyInstance.GetComponent<CharacterMover>();
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
		while (enemyInstance != null && moveState != MoveState.Inactive)
		{
			var stateUpdate = Run.Coroutine(StateUpdate());
			yield return stateUpdate.WaitFor;
		}
	}

	public IEnumerator StateUpdate ()
	{
		if (moveState != MoveState.Inactive)
		{
			enemyInstance.SendMessage("OnCmaeraFollow", enemyInstance, SendMessageOptions.DontRequireReceiver);
		}

		if (moveState == MoveState.Moving && UnitUtil.IsPlayerEncounter(GetCurrentTileKey()))
		{
			// FIXME: Battle with only room owner.
			var playerId = UnitUtil.GetPlayerIdOnTile(GetCurrentTileKey());
			NetworkManager.StartBattleFromEnemy(enemyId, playerId);
			moveState = MoveState.Battle;
		}

		if ((remainMoveCount <= 0 && moveState == MoveState.Moving)||(moveState == MoveState.MakingComplete))
		{
			moveState = MoveState.Inactive;
			GameManager.gameManagerInstance.PassTurnToNextPlayer();
		}
		else if (moveState == MoveState.Moving)
		{
			var borderDictionary = SearchBorderTiles();
			var movableDictionary = SearchMovableTiles(borderDictionary);
            var movableDictionaryWithoutSaveTiles = SearchMovableTilesWithoutSaveTiles(movableDictionary);

            SetDestination(movableDictionaryWithoutSaveTiles);

			var moveAndNotify = Run.Coroutine(MoveAndNotify(toMoveTile));
			yield return moveAndNotify.WaitFor;

			remainMoveCount--;
		}
	}

	public void Kill()
	{
		GameObject.Destroy(enemyInstance.gameObject);
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
