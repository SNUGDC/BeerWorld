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
	private CharacterManager myCharacterManager = null;
	private EnemyPlaceHolder enemyHolder = null;
	public Enemy enemyPrefab;
	private Dictionary<string, EnemyManager> enemies = new Dictionary<string, EnemyManager>();

	public Dictionary<string, EnemyManager> GetEnemies()
	{
		return enemies;
	}

	public IEnumerable<EnemyManager> GetEnemiesList()
	{
		return enemies.Values;
	}

	public static CharacterManager GetMyCharacterManager()
	{
		return gameManagerInstance.myCharacterManager;
	}

	private Dictionary<NetworkViewID, CharacterManager> otherCharacterManagers = new Dictionary<NetworkViewID, CharacterManager>();

	private List<NetworkViewID> otherPlayers = new List<NetworkViewID>();

	public static IEnumerable<CharacterManager> GetAllPlayersEnumerator()
	{
		yield return gameManagerInstance.myCharacterManager;

		foreach (var player in gameManagerInstance.otherCharacterManagers.Values)
		{
			yield return player;
		}
	}

	public static NetworkViewID GetNetworkViewID(CharacterManager player)
	{
		if (gameManagerInstance.isMyCharacterManager(player))
		{
			return NetworkManager.networkInstance.Id;
		}
		else
		{
			return Slinqable.Slinq(gameManagerInstance.otherPlayers).Where(
					(playerId) => {
					var otherPlayer = gameManagerInstance.otherCharacterManagers[playerId];
					return otherPlayer == player;
					}).First();
		}
	}

	public static CharacterManager GetCharacterManager(NetworkViewID id)
	{
		if (null == gameManagerInstance.otherCharacterManagers[id])
		{
			Debug.LogError("Cannot find user of " + id);
		}
		return gameManagerInstance.otherCharacterManagers[id];
	}

	public bool isMyCharacterManager(CharacterManager unitManager)
	{
		return myCharacterManager == unitManager;
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
			EnemyManager turnEnemy = TurnManager.Get().GetTurnEnemy();
			turnEnemy.ChangeMoveStateToIdle();
		}
	}

	public void AddUser(NetworkViewID id)
	{
		if (id == NetworkManager.networkInstance.Id)
		{
			return;
		}

		otherCharacterManagers.Add(id,
				CharacterManager.CreateInStart(characterPrefab, arrowPrefab));
		otherCharacterManagers[id].Init();
		otherPlayers.Add(id);
		TurnManager.Get().AddPlayerTEMP(id);
	}

	public void GameStart()
	{
		var enemyPlaces = enemyHolder.GetEnemyPlaces();
		Slinqable.Slinq(enemyPlaces).ForEach(
				(tileKey) => {
				NetworkManager.MakeEnemy(tileKey);
				}
				);
		NetworkManager.SendGameStartMessage();
	}

	void Awake ()
	{
		gameManagerInstance = this;
		myCharacterManager = CharacterManager.CreateInStart(characterPrefab, arrowPrefab);
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

	public void KillEnemy(string enemyId)
	{
		var enemy = enemies[enemyId];
		enemies.Remove(enemyId);
		enemy.Kill();
	}
}
