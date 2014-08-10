using UnityEngine;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Collections;
using Smooth.Slinq;
using Smooth;

public class GameManager : MonoBehaviour
{
	public static GameManager gameManagerInstance = null;
	public Character characterPrefab;
	public DirectionArrow arrowPrefab;
	private UnitManager myCharacterManager = null;
	private EnemyPlaceHolder enemyHolder = null;
	public Enemy enemyPrefab;
	private Dictionary<string, EnemyManager> enemies = new Dictionary<string, EnemyManager>();

    public Dictionary<string, EnemyManager> GetEnemies()
    {
        return enemies;
    }

	public static UnitManager GetMyCharacterManager()
	{
		return gameManagerInstance.myCharacterManager;
	}

	private Dictionary<NetworkViewID, UnitManager> otherCharacterManagers = new Dictionary<NetworkViewID, UnitManager>();

	private List<NetworkViewID> otherPlayers = new List<NetworkViewID>();

	public static UnitManager GetCharacterManager(NetworkViewID id)
	{
		if (null == gameManagerInstance.otherCharacterManagers[id])
		{
			Debug.LogError("Cannot find user of " + id);
		}
		return gameManagerInstance.otherCharacterManagers[id];
	}

	public void PassTurnToNextPlayer()
	{
		TurnManager.Get().PassTurn();
		TurnManager.State turnState = TurnManager.Get().GetState();

		if (turnState == TurnManager.State.Player)
		{
			GetMyCharacterManager().ChangeMoveStateToIdle();
		}
		else if (turnState == TurnManager.State.OtherPlayer)
		{
			NetworkViewID turnPlayerViewID = TurnManager.Get().GetTurnPlayer();
			NetworkManager.SendTurnStartMessage(turnPlayerViewID);
		}
		else if (turnState == TurnManager.State.Enemy)
		{
			Slinqable.Slinq(enemies.Values).FirstOrNone()
				.ForEach(enemyManager => enemyManager.ChangeMoveStateToIdle());
		}
	}

	public void AddUser(NetworkViewID id)
	{
		if (id == NetworkManager.networkInstance.Id)
		{
			return;
		}

		otherCharacterManagers.Add(id,
			UnitManager.CreateInStart(characterPrefab, arrowPrefab));
		otherCharacterManagers[id].Init();
		otherPlayers.Add(id);
		TurnManager.Get().AddPlayerTEMP(id);
	}

	public void GameStart()
	{
		int enemyStartTileKey = enemyHolder.getEnemyStartTileKey();
		NetworkManager.MakeEnemy(enemyStartTileKey);
		NetworkManager.SendGameStartMessage();
	}

	void Awake ()
	{
		gameManagerInstance = this;
		myCharacterManager = UnitManager.CreateInStart(characterPrefab, arrowPrefab);
		enemyHolder = new EnemyPlaceHolder();
	}

	// Use this for initialization
	void Start () {
		myCharacterManager.Init();
	}

	// Update is called once per frame
	void Update () {
		myCharacterManager.Update();
		Slinqable.Slinq(enemies.Values).ForEach(
            enemyManager => enemyManager.Update());
    }

	public void InstantiateEnemyByNetwork(string enemyId, int tileKey)
	{
		Tile startTile = TileManager.GetExistTile(tileKey);
		EnemyManager enemyManager = EnemyManager.Create(enemyPrefab, startTile, enemyId);

		enemyManager.Init();

		enemies.Add(enemyId, enemyManager);
	}

	public void MoveEnemy(int tileKey, string enemyId)
	{
		Debug.Log("Move enemy " + enemyId + ", to " + tileKey);
		enemies[enemyId].Move(tileKey);
	}

	public Option<string> GetFirstEnemyId()
	{
		return Slinqable.Slinq(enemies.Keys).FirstOrNone();
	}

	public Option<EnemyManager> GetEnemy(string id)
	{
		return enemies.TryGet(id);
	}
}
