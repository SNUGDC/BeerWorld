using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager gameManagerInstance = null;
	public Character characterPrefab;
	public DirectionArrow arrowPrefab;
	private UnitManager myCharacterManager = null;
	private EnemyPlaceHolder enemyHolder = null;
	public Enemy enemyPrefab;

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
			enemyManager.ChangeMoveStateToIdle();
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

	void Awake ()
	{
		gameManagerInstance = this;
		myCharacterManager = UnitManager.CreateInStart(characterPrefab, arrowPrefab);
		enemyHolder = new EnemyPlaceHolder(enemyPrefab);
	}

	public EnemyManager enemyManager;
	// Use this for initialization
	void Start () {
		myCharacterManager.Init();
		enemyHolder.PlaceEnemy(Enemy.EnemyType.Smallest);
		enemyManager = enemyHolder.getEnemyManager();
	}

	// Update is called once per frame
	void Update () {
		myCharacterManager.Update();
		if (enemyManager != null)
		{
			enemyManager.Update();
		}
	}
}
