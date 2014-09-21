using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public enum TileType
	{
		Default,
		Start,
		Item,
		Buff,
		Warp,
		Jail,
		Save
	}

	public bool isVisible;
	[SerializeField]
	private int tileKey = 0;
	//public bool isStartTile;

	public TileType tileType;

	public Sprite defaultTile;
	public Sprite startTile;
	public Sprite itemTile;
	public Sprite buffTile;
	public Sprite warpTile;
	public Sprite jailTile;
	public Sprite saveTile;
	public Sprite saveTileToggled;

	public bool IsStartTile()
	{
		return tileType == TileType.Start;
	}

	public void SetTileKey(int key)
	{
		tileKey = key;
	}

	public int GetTileKey()
	{
		return tileKey;
	}

	public Vector2 GetCoord()
	{
		return FieldTileUtility.GetCoordFromKey(tileKey);
	}

	public void ToggleSaveTile(bool isSet)
	{
		if (tileType != TileType.Save)
		{
			Debug.LogError("Cannot toggle not save tile.");
			return;
		}

		var spriteRenderer = GetComponent<SpriteRenderer>();
		if (isSet)
		{
			spriteRenderer.sprite = saveTileToggled;
		}
		else
		{
			spriteRenderer.sprite = saveTile;
		}
	}
}
