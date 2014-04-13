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
		EditorGUILayout.LabelField ("This is label.");
		if (GUILayout.Button("sample button."))
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
