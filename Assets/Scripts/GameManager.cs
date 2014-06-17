using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager gameManagerInstance = null;
	public Character characterPrefab;
	public DirectionArrow arrowPrefab;
	private CharacterManager myCharacterManager = null;

	List<NetworkViewID> playerList = new List<NetworkViewID>();

	public static CharacterManager GetMyCharacterManager()
	{
		return gameManagerInstance.myCharacterManager;
	}

//	public static CharacterManager charaterManagerInstance;

	public static void AddUserID (NetworkViewID id)
	{
		gameManagerInstance.playerList.Add(id);
	}

	public static void RemoveUserID (NetworkViewID id)
	{
		gameManagerInstance.playerList.Remove(id);
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
