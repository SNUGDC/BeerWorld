using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMover : MonoBehaviour
{
	public int currentTileKey
	{
		get;
		private set;
	}

	public int preTileKey
	{
		get;
		private set;
	}

	public int prePreTileKey
	{
		get;
		private set;
	}

    public int GetCurrentTileKey()
    {
        return currentTileKey;
    }

	public void InitializeTileKey(int tileKey)
	{
		currentTileKey = tileKey;
		prePreTileKey = 0;
		preTileKey = 0;
	}

	public void UpdateTileKey(int tileKey)
	{
		prePreTileKey = preTileKey;
		preTileKey = currentTileKey;
		currentTileKey = tileKey;
	}

	public IEnumerator MoveTo(int tileKey)
	{
		Tile tile = TileManager.GetExistTile(tileKey);
		return MoveTo(tile);
	}

	public IEnumerator MoveTo(int tileCoordX, int tileCoordY)
	{
		Tile tile = TileManager.GetTileByCoord(tileCoordX, tileCoordY);
		return MoveTo(tile);
	}

	public IEnumerator MoveTo(Tile toMoveTile)
	{
		//iTween.MoveTo(
		Vector3 nextTilePosition = new Vector3(
				toMoveTile.transform.position.x,
				toMoveTile.transform.position.y,
				transform.position.z);
		Vector3 currentPosition = transform.position;
		Vector3 diff = currentPosition - nextTilePosition;
		float moveTime = diff.magnitude / DelayManager.Get().playerMoveSpeed;

		iTween.MoveTo(gameObject,
			iTween.Hash("position", nextTilePosition, "time", moveTime, "easeType", DelayManager.Get().moveEaseType)
		);
		yield return Run.WaitSeconds(moveTime).WaitFor;

		int currentTileKey = FieldTileUtility.GetKeyFromTile(toMoveTile);
		UpdateTileKey(currentTileKey);
	}

	bool IsPreTile(Tile tile)
	{
		int tileKeyOfBorderTile = FieldTileUtility.GetKeyFromTile(tile);

		return preTileKey == tileKeyOfBorderTile;
	}

	bool IsPrePreTile(Tile tile)
	{
		int tileKeyOfBorderTile = FieldTileUtility.GetKeyFromTile(tile);

		return prePreTileKey == tileKeyOfBorderTile;
	}

	public Dictionary<TileManager.TileDirection, Tile> GetTileDictionaryOfMovableTiles(Dictionary<TileManager.TileDirection, Tile> borderDictionary)
	{
		Dictionary<TileManager.TileDirection, Tile> movableDictionary = new Dictionary<TileManager.TileDirection, Tile>();

		TileManager.TileDirection direction;
		Tile tile;

		foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in borderDictionary)
		{
			direction = pair.Key;
			tile = pair.Value;

			if (tile == null)
			{
				continue;
			}

			if (IsPreTile(tile) == true)
			{
				continue;
			}

			if (IsPrePreTile(tile) == true)
			{
				continue;
			}

			if (tile.tileType == Tile.TileType.Start)
			{
				continue;
			}

			movableDictionary.Add(direction, tile);
		}

		return movableDictionary;
	}
}
