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

	private void EnemyTurn()
	{
		Debug.LogWarning("Enemy Turn Start");
		//PassTurn();
	}

	public void PassTurn()
	{
		if (state == State.Player)
		{
			if (otherPlayers.Count == 0)
			{
				state = State.Enemy;
				EnemyTurn();
			}
			else
			{
				state = State.OtherPlayer;
				currentTurnIndex = 0;
			}
		}
		else if (state == State.OtherPlayer)
		{
			currentTurnIndex += 1;
			if (currentTurnIndex >= otherPlayers.Count)
			{
				state = State.Enemy;
				currentTurnIndex = 0;
				EnemyTurn();
			}
		}
		else
		{
			state = State.Player;
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
}
