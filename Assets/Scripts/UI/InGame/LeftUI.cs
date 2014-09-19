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
		//SerializeField is used for debugging in unity.
		[SerializeField]
		private GameObject playerUI;
		[SerializeField]
		private NetworkViewID playerId;
		[SerializeField]
		private SpriteRenderer life;
		[SerializeField]
		private SpriteRenderer buff;

		public Player(GameObject playerUI, NetworkViewID playerId)
		{
			this.playerUI = playerUI;
			this.playerId = playerId;
			this.life = FindGameObjectByName.FindChild(playerUI, "life").GetComponent<SpriteRenderer>();
			this.buff = FindGameObjectByName.FindChild(playerUI, "buff").GetComponent<SpriteRenderer>();

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

		public void SetBuff(bool isBuff, int turn)
		{
			if (turn < 1)
			{
				buff.enabled = false;
				return;
			}
			else
			{
				buff.enabled = true;
			}

			var uiManager = BattleUIManager.Get();
			if (isBuff)
			{
				var spriteId = uiManager.buffSprites.Count - Mathf.Clamp(turn, 1, uiManager.buffSprites.Count);

				buff.sprite = BattleUIManager.Get().buffSprites[spriteId];
			}
			else
			{
				var spriteId = uiManager.deBuffSprites.Count - Mathf.Clamp(turn, 1, uiManager.deBuffSprites.Count);

				buff.sprite = BattleUIManager.Get().deBuffSprites[spriteId];
			}
		}
	}
}
