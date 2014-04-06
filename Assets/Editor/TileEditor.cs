using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor {

	public override void OnInspectorGUI()
	{
        Tile tile = target as Tile;

		base.OnInspectorGUI ();

		if (GUI.changed)
        {
            Debug.LogWarning ("Tile gui changed.");
            tile.gameObject.SetActive(tile.isVisible);
            EditorUtility.SetDirty (target);
        }
	}
}
