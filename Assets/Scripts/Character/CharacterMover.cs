using UnityEngine;
using System.Collections;

public class CharacterMover : MonoBehaviour
{
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
        Vector2 nextTileCoordinate =
            FieldTileUtility.GetCoordFromPosition(
                nextTilePosition.x, 
                nextTilePosition.y);

        transform.position = new Vector3(nextTilePosition.x, nextTilePosition.y, Character.Depth);
        Vector2 newCoordinate = FieldTileUtility.GetCoordFromPosition(nextTilePosition.x, nextTilePosition.y);
    }
}
