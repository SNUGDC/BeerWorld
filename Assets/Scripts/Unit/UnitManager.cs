using UnityEngine;
using System.Collections.Generic;
using Smooth.Slinq;

public class UnitManager
{
    public int GetCurrentTileKey()
    {
        return characterMover.GetCurrentTileKey();
    }

	private Tile spawnTile = null;
	public static UnitManager CreateInStart(Unit unitPrefab, DirectionArrow arrowPrefab)
	{
		Tile startTile = TileManager.GetStartTile ();
		return new UnitManager(unitPrefab, arrowPrefab, startTile);
	}

	public static UnitManager Create(Unit unitPrefab, DirectionArrow arrowPrefab, Tile spawnTile)
	{
		return new UnitManager(unitPrefab, arrowPrefab, spawnTile);
	}

	private UnitManager(Unit unitPrefab, DirectionArrow arrowPrefab, Tile spawnTile)
	{
		this.unitPrefab = unitPrefab;
		this.arrowPrefeb = arrowPrefab;
		this.spawnTile = spawnTile;
	}

	public void Init()
	{
		Start();
	}

	private Unit unitPrefab;
	private DirectionArrow arrowPrefeb;
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
        Moving,
        Waiting,
        DirectionSelected
    }
    [SerializeField]
    private MoveState moveState = MoveState.Inactive;

    public UnitManager.MoveState GetMoveState()
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
    	Vector3 position = unitInstance.transform.position;
    	Vector2 unitCoordinate = FieldTileUtility.GetCoordFromPosition(position.x, position.y);
    	return TileManager.GetTileDictionaryOfBorderTiles(unitCoordinate);
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

    		Vector3 unitPosition = unitInstance.transform.position;
    		Vector2 arrowCoordinate = FieldTileUtility.GetCoordOfDirectionByPosition(direction, unitPosition);
    		Vector2 arrowPosition = FieldTileUtility.GetPositionFromCoordinate(arrowCoordinate.x, arrowCoordinate.y);
    		Vector3 arrowPositionWithZ = new Vector3 (arrowPosition.x, arrowPosition.y, unitPosition.z);

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

    bool isThereEnemy(Tile destinationTile)
    {
        return Slinqable.Slinq(GameManager.gameManagerInstance.GetEnemies().Values).Where(
            enemyManager => enemyManager.GetCurrentTileKey() == GetCurrentTileKey())
            .Count() > 0;
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
			if (NetworkManager.isConnected())
			{
				NetworkManager.SendTurnEndMessage();
			} else {
				GameManager.gameManagerInstance.PassTurnToNextPlayer();
			}

			return;
		}

		if (moveState == MoveState.Moving)
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
				SetDestination(movableDictionary);
				MoveAndNotify(toMoveTile);
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

        	MoveAndNotify(toMoveTile);
        	howManyMove--;

        	moveState = MoveState.Moving;
        }
    }
}
