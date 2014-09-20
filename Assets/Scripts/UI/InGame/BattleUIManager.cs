using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

public class BattleUIManager : MonoBehaviour
{
	public Camera uiCamera;
	public LeftUI leftUI;
	public InventoryUI inventoryUI;

	public List<LeftUIComps.Player> players;
	public List <Sprite> heartSprites;
	public List <Sprite> buffSprites;
	public List <Sprite> deBuffSprites;
	public List <ItemSpriteSet> itemSprites;

	[System.Serializable]
	public class ItemSpriteSet
	{
		public Character.Item item;
		public Sprite sprite;
	}

	private static BattleUIManager instance = null;
	public static BattleUIManager Get()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		inventoryUI.itemCardComps = new List<InventoryComps.ItemCard>();

		inventoryUI.itemCards.ForEach((itemCardGO) => {
			var newItemCardComp = new InventoryComps.ItemCard(Character.Item.None, itemCardGO);
			inventoryUI.itemCardComps.Add(newItemCardComp);
		});
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

	public void SetPlayerTurn(NetworkViewID playerId)
	{
		var playTurnAnimation = leftUI.leftUIGO.GetComponent<PlayerTurnAnimation>();
		var turnIndex = players.FindIndex((player) => player.GetId() == playerId);
		if (turnIndex == -1)
		{
			Debug.LogError("Cannot find plyaer");
		}
		else
		{
			playTurnAnimation.SetTurn(turnIndex);
		}
	}

	public void SetEnemyTrn()
	{
		var playTurnAnimation = leftUI.leftUIGO.GetComponent<PlayerTurnAnimation>();
		playTurnAnimation.SetTurn(3);
	}

	public void ShowBuffStartAnimation(NetworkViewID playerId, Vector3 tilePos)
	{
		var playerIndex = players.FindIndex((player) => player.GetId() == playerId);
		var buffAnimation = leftUI.leftUIGO.GetComponent<BuffAnimation>();
		buffAnimation.PlayBuffAt(tilePos, playerIndex);
	}

	Sprite GetSpriteOfItem(Character.Item item)
	{
		var sprite = Slinqable.Slinq(itemSprites).FirstOrNone(
			(itemSprite) => itemSprite.item == item
			)
		.Select(
			(itemSprite) => itemSprite.sprite
			)
		.ValueOr(
			() => { throw new System.Exception("cannot get item sprite"); }
			);

		return sprite;
	}

	public void AddItemCard(Character.Item item)
	{
		Slinqable.Slinq(inventoryUI.itemCardComps).FirstOrNone(
			(itemCard) => itemCard.GetItem() == Character.Item.None
		).ForEachOr(
			(itemCard) => {
				var sprite = GetSpriteOfItem(item);
				itemCard.SetItem(item, sprite);
			},
			() => Debug.Log("There is no empty imte.")
		);
	}

	Run ShowItemUseAnimation(Character.Item item)
	{
		inventoryUI.itemCardUseEffect.SetActive(true);
		var effectSpriteRenderer = inventoryUI.itemCardUseEffect.GetComponent<SpriteRenderer>();
		effectSpriteRenderer.sprite = GetSpriteOfItem(item);
		var animator = inventoryUI.itemCardUseEffect.GetComponent<Animator>();
		animator.SetTrigger("ShowItemUseEffect");

		return Run.After(0.3f, () => { })
		.Then(
			Run.WaitWhile(() => effectSpriteRenderer.enabled, false)
		)
		.ExecuteWhenDone(
			() => Debug.Log("Use item effect done.")
		)
		.ExecuteWhenDone(
			() => Debug.Log("JEWIOJIOEJFW")
		);
	}

	void RemoveItem(Character.Item item)
	{
		Slinqable.Slinq(inventoryUI.itemCardComps)
			.FirstOrNone((itemCard) => itemCard.GetItem() == item)
			.ForEachOr(
				(itemCard) => itemCard.SetItem(Character.Item.None, null),
				() => { throw new Exception("Cannot get item"); }
			);
	}

	public void UseItemCard(Character.Item item)
	{
		ShowItemUseAnimation(item)
			.ExecuteWhenDone(() => {
				Debug.Log("Not delete item");
				var characterManager = GameManager.GetMyCharacterManager();
				var character = characterManager.GetCharacterInstance();
				character.UseItem(item);
				RemoveItem(item);
			});
	}
}
