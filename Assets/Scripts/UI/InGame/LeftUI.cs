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
		private SpriteRenderer life;

		public Player(GameObject playerUI, NetworkViewID playerId)
		{
			this.playerUI = playerUI;
			this.playerId = playerId;
			this.life = FindGameObjectByName.FindChild(playerUI, "life").GetComponent<SpriteRenderer>();

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
			life.sprite = BattleUIManager.Get().heartSprites[currentHp];
		}
	}
}
