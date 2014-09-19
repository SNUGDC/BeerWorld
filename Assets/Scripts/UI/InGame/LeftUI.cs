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
		[SerializeField]
		private TextMesh hpText;

		public Player(GameObject playerUI, NetworkViewID playerId)
		{
			this.playerUI = playerUI;
			this.playerId = playerId;
			this.hpText = FindGameObjectByName.FindChild(playerUI, "hp").GetComponent<TextMesh>();

			EnableUI();
		}

		public NetworkViewID GetId()
		{
			return playerId;
		}

		void EnableUI()
		{
			playerUI.SetActive(true);
		}

		public void SetHp(int currentHp)
		{
			Debug.Log("Set hp to " + currentHp);
		}
	}
}
