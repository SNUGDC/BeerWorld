using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor {

	public override void OnInspectorGUI()
	{
        Tile tile = target as Tile;
        SpriteRenderer spriteRenderer = tile.gameObject.GetComponent<SpriteRenderer>();

		base.OnInspectorGUI ();

		if (GUI.changed)
        {
            if (tile.isVisible == true)
            {
                Debug.LogWarning ("Tile is visible.");
            }
            else if (tile.isVisible == false)
            {
                Debug.LogWarning ("Tile is invisible.");
            }

            switch(tile.tileType)
            {
                case Tile.TileType.Default:
                    spriteRenderer.sprite = tile.defaultTile;
                    Debug.LogWarning ("defaultTile.");
                    break;
                case Tile.TileType.Start:
                    spriteRenderer.sprite = tile.startTile;
                    Debug.LogWarning ("startTile.");
                    break;
                case Tile.TileType.Item:
                    spriteRenderer.sprite = tile.itemTile;
                    Debug.LogWarning ("itemTile.");
                    break;
                case Tile.TileType.Save:
                    spriteRenderer.sprite = tile.saveTile;
                    Debug.LogWarning ("saveTile.");
                    break;
                case Tile.TileType.Warp:
                    spriteRenderer.sprite = tile.warpTile;
                    Debug.LogWarning ("warpTile.");
                    break;
                case Tile.TileType.Buff:
                    spriteRenderer.sprite = tile.buffTile;
                    Debug.LogWarning ("buffTile.");
                    break;
                case Tile.TileType.Jail:
                    spriteRenderer.sprite = tile.jailTile;
                    Debug.LogWarning ("jailTile.");
                    break;
            }

            tile.gameObject.SetActive(tile.isVisible);
            EditorUtility.SetDirty (target);
        }
	}
}
