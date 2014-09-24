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

	private IEnumerator MoveToIter(Tile toMoveTile)
	{
		Vector3 nextTilePosition = new Vector3(
				toMoveTile.transform.position.x,
				toMoveTile.transform.position.y,
				transform.position.z);

		while (true)
		{
			var diff = nextTilePosition - transform.position;
			if (diff.magnitude <= float.Epsilon)
			{
				break;
			}

			transform.position = Vector3.MoveTowards(transform.position, nextTilePosition, DelayManager.Get().playerMoveSpeed * Time.deltaTime);

			yield return null;
		}

		int currentTileKey = FieldTileUtility.GetKeyFromTile(toMoveTile);
		UpdateTileKey(currentTileKey);
	}

	Queue<Run> moveAnimationQueue = new Queue<Run>();
	public IEnumerator MoveTo(Tile toMoveTile)
	{
		yield return Run.WaitWhile(() => moveAnimationQueue.Count > 0).WaitFor;

		var animation = Run.Coroutine(MoveToIter(toMoveTile));
		moveAnimationQueue.Enqueue(animation);

		yield return animation.WaitFor;

		moveAnimationQueue.Dequeue();
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
