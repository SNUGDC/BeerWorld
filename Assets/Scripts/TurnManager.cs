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
	public int turnCount = 15;

    public int MaxTurn = 1;

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
        var enemies = GameManager.gameManagerInstance.GetEnemiesListSizeOrdered();
		waitingEnemies = new Queue<EnemyManager>(enemies);
	}

	public Run PassTurn()
	{
		//FIXME: just waiting 1 second for animation.
		Run waiting = Run.WaitSeconds(0);
		if (state == State.Player)
		{
			currentTurnIndex = 0;
			if (otherPlayers.Count == 0)
			{
				NetworkManager.ShowEnemyTurn();
				waiting = waiting.Then(() => Run.WaitSeconds(1f));
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
				NetworkManager.ShowEnemyTurn();
				waiting = waiting.Then(() => Run.WaitSeconds(1f));
				currentTurnIndex = 0;
				state = State.Enemy;
				EnemyTurnStart();
			}
		}
		else
		{
			if (waitingEnemies.Count <= 0)
			{
				CountUpTurn();
				waiting = waiting.Then(() => Run.WaitSeconds(1f));
				state = State.Player;
			}
		}
		return waiting;
	}

	private void CountUpTurn()
	{
        turnCount += 1;
		NetworkManager.SyncTurnCount(turnCount);

        Debug.Log((turnCount > MaxTurn));
        Debug.Log(!(GameManager.gameManagerInstance.IsRemainMiddleEnemy()));

        if ((turnCount > MaxTurn) || !(GameManager.gameManagerInstance.IsRemainMiddleEnemy()))
        {
            NetworkManager.SendPopGameOverImg();
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
