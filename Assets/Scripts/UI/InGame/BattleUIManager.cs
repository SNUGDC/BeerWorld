using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

public class BattleUIManager : MonoBehaviour
{
	public LeftUI leftUI;
	public List<LeftUIComps.Player> players;

	private static BattleUIManager instance = null;
	public static BattleUIManager Get()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	public void SetPlayers(List<NetworkViewID> playerIds)
	{
		if (playerIds.Count > leftUI.playerUIs.Count)
		{
			Debug.LogError("Not enough UI slot for character");
		}

		Queue<GameObject> playerUIs = new Queue<GameObject>(leftUI.playerUIs);
		var players = Slinqable.Slinq(playerIds).Select((playerId) => {
			var playerUI = playerUIs.Dequeue();
			return new LeftUIComps.Player(playerUI, playerId);
		});

		this.players = players.ToList();
	}

	public LeftUIComps.Player GetPlayerUI(NetworkViewID playerId)
	{
		Option<LeftUIComps.Player> optionalPlayer = Slinqable.Slinq(players).FirstOrNone((player) => player.GetId() == playerId);

		return optionalPlayer.ValueOr(() => {
			throw new Exception("CannotGetUI");
		});
	}
}
