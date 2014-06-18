using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager gameManagerInstance = null;
	public Character characterPrefab;
	public DirectionArrow arrowPrefab;
	private CharacterManager myCharacterManager = null;

	private Dictionary<NetworkViewID, CharacterManager> otherCharacterManagers = new Dictionary<NetworkViewID, CharacterManager>();
	//private List<NetworkViewID> playerList = new List<NetworkViewID>();
	private Dictionary<int, NetworkViewID> otherPlayers = new Dictionary<int, NetworkViewID>();
	private int[] indexOfPlayers = new int[4] {0, 0, 0, 0};

	private int indexOfActivePlayer;

	public static int getIndexOfActivePlayer()
	{
		return gameManagerInstance.indexOfActivePlayer;
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

		PrintLog();
	}

	void PrintLog()
	{
		Debug.Log("indexOfPlayers(Array) = { ");
		for (int i = 0; i < 4; i++)
		{
			Debug.Log("indexOfPlayers, i th : " + indexOfPlayers[i]);
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
			if (indexOfPlayers[i] == 0)
			{
				indexOfPlayers[i] = i;
				return i;
			}
		}
		return 0;
	}

	void AddIndex(int index)
	{

	}

	void Awake ()
	{
		gameManagerInstance = this;
		myCharacterManager = new CharacterManager(characterPrefab, arrowPrefab);
	}

	// Use this for initialization
	void Start () {
		myCharacterManager.Init();
	}
	
	// Update is called once per frame
	void Update () {
		myCharacterManager.Update();
	}
}
