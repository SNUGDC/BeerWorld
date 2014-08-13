using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
	private static TurnManager instance;

	public static TurnManager Get()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	//private List<NetworkViewID> playerList = new List<NetworkViewID>();
	public enum State
	{
		Player,
		OtherPlayer,
		Enemy
	}

	private State state = State.Player;
	private int currentTurnIndex;
	private List<NetworkViewID> otherPlayers = new List<NetworkViewID>();

	public void AddPlayerTEMP(NetworkViewID otherPlayer)
	{
		otherPlayers.Add(otherPlayer);
	}

	public void Initialize(List<NetworkViewID> otherPlayers)
	{
		this.otherPlayers = otherPlayers;
	}

	Queue<EnemyManager> waitingEnemies;
	private void EnemyTurnStart()
	{
		var enemies = GameManager.gameManagerInstance.GetEnemiesList();
		waitingEnemies = new Queue<EnemyManager>(enemies);
	}

	public void PassTurn()
	{
		if (state == State.Player)
		{
			currentTurnIndex = 0;
			if (otherPlayers.Count == 0)
			{
				state = State.Enemy;
				EnemyTurnStart();
			}
			else
			{
				state = State.OtherPlayer;
			}
		}
		else if (state == State.OtherPlayer)
		{
			currentTurnIndex += 1;
			if (currentTurnIndex >= otherPlayers.Count)
			{
				currentTurnIndex = 0;
				state = State.Enemy;
				EnemyTurnStart();
			}
		}
		else
		{
			if (waitingEnemies.Count > 0)
			{
				return;
			}
			else
			{
				state = State.Player;
			}
		}
	}

	public State GetState()
	{
		return state;
	}

	public NetworkViewID GetTurnPlayer()
	{
		return otherPlayers[currentTurnIndex];
	}

	public EnemyManager GetTurnEnemy()
	{
		var enemy = waitingEnemies.Dequeue();
		return enemy;
	}
}
