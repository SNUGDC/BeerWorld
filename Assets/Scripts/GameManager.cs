using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager gameManagerInstance = null;
	public Character characterPrefab;
	public DirectionArrow arrowPrefab;
	private CharacterManager myCharacterManager = null;
    private EnemyPlaceHolder enemyHolder = null;
    public Enemy enemyPrefab;

	private Dictionary<NetworkViewID, CharacterManager> otherCharacterManagers = new Dictionary<NetworkViewID, CharacterManager>();
	//private List<NetworkViewID> playerList = new List<NetworkViewID>();
	private Dictionary<int, NetworkViewID> otherPlayers = new Dictionary<int, NetworkViewID>();
	private int[] ExistingPlayersArray = new int[4] {1, 0, 0, 0};

	private int turnOfActivePlayer = 0;

	public static int getTurnOfActivePlayer()
	{
		return gameManagerInstance.turnOfActivePlayer;
	}

	public void PassTurnToNextPlayer()
	{
		NetworkViewID nextPlayerId;
		turnOfActivePlayer = FindNextExistingPlayer(turnOfActivePlayer);

		if (turnOfActivePlayer != 0)
		{
			nextPlayerId = otherPlayers[turnOfActivePlayer];
			Debug.Log("nextTurnIndex : " + turnOfActivePlayer + ", nextPlayerId : " + nextPlayerId);
		}
		else 
		{
			nextPlayerId = NetworkManager.networkInstance.GetNetworkID();
			Debug.Log("There is no other player.");
		}
		NetworkManager.SendTurnStartMessage (nextPlayerId);
	}

	int FindNextExistingPlayer (int currentTurnOfActivePlayer)
	{
		if (currentTurnOfActivePlayer == ExistingPlayersArray.Length - 1)
		{
			return 0;
		}

		int nextTurnIndex = currentTurnOfActivePlayer + 1;
		for(;nextTurnIndex < ExistingPlayersArray.Length; nextTurnIndex += 1)
		{
			if (ExistingPlayersArray[nextTurnIndex] != 0)
			{
				return nextTurnIndex;
			}
		}

		return 0;		
	}

	public static CharacterManager GetMyCharacterManager()
	{
		return gameManagerInstance.myCharacterManager;
	}

	public static CharacterManager GetCharacterManager(NetworkViewID id)
	{
		if (null == gameManagerInstance.otherCharacterManagers[id])
		{
			Debug.LogError("Cannot find user of " + id);
		}
		return gameManagerInstance.otherCharacterManagers[id];
	}

	public void AddUser(NetworkViewID id)
	{
		otherCharacterManagers.Add(id,
			new CharacterManager(characterPrefab, arrowPrefab));
		otherCharacterManagers[id].Init();

		int userIndex = MakeIndex();

		if (userIndex == 0)
		{
			Debug.LogError("This room is full.");
		}
		otherPlayers.Add(userIndex, id); 

		//PrintLog();
	}

	void PrintLog()
	{
		Debug.Log("ExistingPlayersArray(Array) = { ");
		for (int i = 0; i < 4; i++)
		{
			Debug.Log("ExistingPlayersArray, i th : " + ExistingPlayersArray[i]);
		}
		foreach (KeyValuePair<int, NetworkViewID> pair in otherPlayers)
		{
			Debug.Log("otherPlayers < Key : " + pair.Key + ", Value : " + pair.Value + " > ");
		}
	}

	int MakeIndex()
	{
		for (int i = 1; i < 4; i++)
		{
			if (ExistingPlayersArray[i] == 0)
			{
				ExistingPlayersArray[i] = i;
				return i;
			}
		}
		return 0;
	}

	void Awake ()
	{
		gameManagerInstance = this;
		myCharacterManager = new CharacterManager(characterPrefab, arrowPrefab);
        enemyHolder = new EnemyPlaceHolder(enemyPrefab);
	}

	// Use this for initialization
	void Start () {
		myCharacterManager.Init();
        enemyHolder.PlaceEnemy();
	}
	
	// Update is called once per frame
	void Update () {
		myCharacterManager.Update();
	}
}
