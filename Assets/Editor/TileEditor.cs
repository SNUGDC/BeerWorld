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
            if (tile.isStartTile == true)
            {
                spriteRenderer.sprite = tile.startTile;
                Debug.LogWarning ("startTile.");
            }
            else if (tile.isStartTile == false)
            {
                spriteRenderer.sprite = tile.defaultTile;
                Debug.LogWarning ("not startTile.");
            }
            tile.gameObject.SetActive(tile.isVisible);
            EditorUtility.SetDirty (target);
        }
	}
}
