using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LeftUI
{
	public List<GameObject> playerUIs;
}

namespace LeftUIComps
{
	[System.Serializable]
	public class Player
	{
		[SerializeField]
		private GameObject playerUI;
		[SerializeField]
		private NetworkViewID playerId;

		public Player(GameObject playerUI, NetworkViewID playerId)
		{
			this.playerUI = playerUI;
			this.playerId = playerId;

			EnableUI();
		}

		void EnableUI()
		{
			playerUI.SetActive(true);
		}
	}
}
