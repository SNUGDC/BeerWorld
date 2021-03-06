using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

[System.Serializable]
public class LeftUI
{
	public List<GameObject> playerUIs;
	public GameObject leftUIGO;
	public List<ClassSprite> classSprite;

	[System.Serializable]
	public class ClassSprite
	{
		public Character.CharClass charClass;
		public Sprite sprite;
	}
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
		private SpriteRenderer portrait;
		[SerializeField]
		private SpriteRenderer life;
		[SerializeField]
		private SpriteRenderer buff;
		[SerializeField]
		private Character.CharClass charClass;
		[SerializeField]
		private SpriteRenderer backgroundForOwner;

		public Player(GameObject playerUI, NetworkViewID playerId, Character.CharClass playerClass)
		{
			this.playerUI = playerUI;
			this.playerId = playerId;
			this.portrait = playerUI.GetComponent<SpriteRenderer>();
			this.life = FindGameObjectByName.FindChild(playerUI, "life").GetComponent<SpriteRenderer>();
			this.buff = FindGameObjectByName.FindChild(playerUI, "buff").GetComponent<SpriteRenderer>();
			this.charClass = playerClass;
			this.backgroundForOwner = FindGameObjectByName.FindChild(playerUI, "myPlayer").GetComponent<SpriteRenderer>();

			SetPortrait();
			EnableUI();
			if (playerId == NetworkManager.Get().GetNetworkID())
			{
				backgroundForOwner.enabled = true;
			}
			else
			{
				backgroundForOwner.enabled = false;
			}
		}

		public void SetPortrait()
		{
			Option<Sprite> portraitSprite = Slinqable.Slinq(BattleUIManager.Get().leftUI.classSprite)
				.FirstOrNone((classSprite) => classSprite.charClass == this.charClass)
				.Select((classSprite) => classSprite.sprite);

			portraitSprite.ForEachOr(
				(sprite) => this.portrait.sprite = sprite,
				() => Debug.LogError("Cannot find sprite for left ui.")
			);
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
			if (currentHp < 1)
			{
				life.enabled = false;
			}
			else
			{
				life.enabled = true;
				var imageIndex = currentHp - 1;
				life.sprite = BattleUIManager.Get().heartSprites[imageIndex];
			}
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

			var buffSpriteIndex = 3; // transparent background.
			if (turn >= 3)
			{
				buffSpriteIndex = 0;
			}
			else if (turn == 2)
			{
				buffSpriteIndex = 1;
			}
			else if (turn == 1)
			{
				buffSpriteIndex = 2;
			}

			var uiManager = BattleUIManager.Get();
			if (isBuff)
			{
				buff.sprite = BattleUIManager.Get().buffSprites[buffSpriteIndex];
			}
			else
			{
				buff.sprite = BattleUIManager.Get().deBuffSprites[buffSpriteIndex];
			}
		}
	}
}
