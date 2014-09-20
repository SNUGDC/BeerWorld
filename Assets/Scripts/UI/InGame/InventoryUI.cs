using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
	public List<GameObject> itemCards;
	public List<InventoryComps.ItemCard> itemCardComps;
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

		public ItemCard(Character.Item item, GameObject itemCardGO)
		{
			this.item = item;
			this.itemCardGO = itemCardGO;

			itemCardGO.SetActive(false);
			itemCardGO.GetComponent<UIButtonMessage>().buttonClickEvent = OnButtonClicked;
		}

		public void OnButtonClicked()
		{
			Debug.Log("Item card cicked.");
		}
	}
}
