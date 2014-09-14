using System;
using Smooth.Algebraics;
using UnityEngine;
using System.Collections.Generic;

namespace BossBattle
{
	public class TurnManager
	{
		public enum Turn
		{
			Player,
			Enemy
		}

		int phaseCount = 0;
		Turn currentTurn = Turn.Player;
		Queue<NetworkViewID> turnPlayers;
		Option<NetworkViewID> currentPlayer = Option<NetworkViewID>.None;

		public delegate List<NetworkViewID> PlayersGetter();
		PlayersGetter playersGetter;

		public TurnManager(PlayersGetter playersGetter)
		{
			turnPlayers = new Queue<NetworkViewID>();
			this.playersGetter = playersGetter;
		}

		void EnterState(Turn turn)
		{
			if (turn == Turn.Player)
			{
				turnPlayers = new Queue<NetworkViewID>(playersGetter());
				currentPlayer = Option.Create(turnPlayers.Dequeue());
			}
		}

		void ExitState(Turn turn)
		{
			if (turn == Turn.Player)
			{
				currentPlayer = Option<NetworkViewID>.None;
				turnPlayers = null;
			}
		}

		void ChangeState(Turn turn)
		{
			ExitState(currentTurn);
			currentTurn = turn;
			EnterState(turn);
		}

		public void PassTurn()
		{
			if (currentTurn == Turn.Player)
			{
				Debug.Log("Count is " + turnPlayers.Count);
				if (turnPlayers.Count == 0)
				{
					ChangeState(Turn.Enemy);
				}
				else
				{
					currentPlayer = Option.Create(turnPlayers.Dequeue());
				}
			}
			else
			{
				ChangeState(Turn.Player);
				EndPhase();
			}
		}

		void EndPhase()
		{
			phaseCount += 1;
		}

		public void StartGame()
		{
			currentTurn = Turn.Player;
			EnterState(Turn.Player);
		}

		public Turn GetCurrentTurn()
		{
			return currentTurn;
		}

		public NetworkViewID GetCurrentPlayer()
		{
			return currentPlayer.ValueOr(
					() => {
					throw new Exception("Cannot get current turn player.");
					});
		}
	}
}
