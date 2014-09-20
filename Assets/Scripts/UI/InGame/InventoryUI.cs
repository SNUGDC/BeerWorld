using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
	public List<GameObject> itemCards;
	public List<InventoryComps.ItemCard> itemCardComps;
	public GameObject itemCardUseEffect;
}

namespace InventoryComps
{
	// serialize for debugging.
	[System.Serializable]
	public class ItemCard
	{
		[SerializeField]
		private Character.Item item;
		[SerializeField]
		private GameObject itemCardGO;
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		public Character.Item GetItem()
		{
			return item;
		}

		public void SetItem(Character.Item item, Sprite sprite)
		{
			this.item = item;
			if (item != Character.Item.None)
			{
				itemCardGO.SetActive(true);
				spriteRenderer.sprite = sprite;
			}
		}

		public ItemCard(Character.Item item, GameObject itemCardGO)
		{
			this.item = item;
			this.itemCardGO = itemCardGO;

			itemCardGO.GetComponent<UIButtonMessage>().buttonClickEvent = OnButtonClicked;
			spriteRenderer = itemCardGO.GetComponentInChildren<SpriteRenderer>();
			itemCardGO.SetActive(false);
		}

		public void OnButtonClicked()
		{
			BattleUIManager.Get().UseItemCard(item);
			Debug.Log("Item card cicked.");
		}
	}
}
