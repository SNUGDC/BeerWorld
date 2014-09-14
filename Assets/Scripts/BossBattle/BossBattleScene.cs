using UnityEngine;
using System.Collections.Generic;
using BTurnManager = BossBattle.TurnManager;

public class BossBattleScene : MonoBehaviour
{
	public static BossBattleScene Instance;
	List<NetworkViewID> players = new List<NetworkViewID>();
	BTurnManager turnManager;

	void Awake()
	{
		Instance = this;
		turnManager = new BTurnManager(
			() => players);
	}

	void Start()
	{
		if (Network.isServer)
		{
			NetworkManager.RequestPlayerData();
		}
	}

	void PassTurn()
	{
		turnManager.PassTurn();
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.A))
		{
			BTurnManager.Turn turn = turnManager.GetCurrentTurn();
			if (turn == BTurnManager.Turn.Enemy)
			{
				Debug.Log("EnemyTurn start.");
			}
			else
			{
				Debug.Log("PlayerTurn start.");
			}
			PassTurn();
		}
	}

	public void AddUser(NetworkViewID playerId)
	{
		players.Add(playerId);

		if (Network.isServer)
		{
			if (Network.connections.Length + 1 == players.Count)
			{
				StartBossBattle();
			}
		}
	}

	public void StartBossBattle()
	{
		turnManager.StartGame();
		Debug.Log("BossBattle Started.");
	}
}
