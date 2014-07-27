using UnityEngine;
using System.Collections;

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

    public void MoveTo(int tileKey)
    {
        Tile tile = TileManager.GetExistTile(tileKey);
        MoveTo(tile);
    }

    public void MoveTo(int tileCoordX, int tileCoordY)
    {
        Tile tile = TileManager.GetTileByCoord(tileCoordX, tileCoordY);
        MoveTo(tile);
    }

    public void MoveTo(Tile toMoveTile)
    {
        Vector2 nextTilePosition = new Vector2(
            toMoveTile.transform.position.x, 
            toMoveTile.transform.position.y);

        transform.position = new Vector3(nextTilePosition.x, nextTilePosition.y, Character.Depth);
				int currentTileKey = FieldTileUtility.GetKeyFromTile(toMoveTile);
				UpdateTileKey(currentTileKey);
    }
}
