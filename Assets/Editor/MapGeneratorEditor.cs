using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapGeneratorEditor : EditorWindow {

	[MenuItem("Editor/Map Editor")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(MapGeneratorEditor));
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField ("Map Generator");
		if (GUILayout.Button("15x15 Generating."))
        {
            FieldTilesMaker tileGenerator = GameObject.FindObjectOfType<FieldTilesMaker> ();
            if (tileGenerator == null)
            {
                Debug.LogWarning("TileGenerator not found.");
            }
            else
            {
                tileGenerator.GenerateTile();
                Debug.LogWarning("Tilegenerator found.");
            }
        }
	}
}
