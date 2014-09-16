using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
}
