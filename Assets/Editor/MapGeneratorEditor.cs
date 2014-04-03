﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapGeneratorEditor : EditorWindow {

	[MenuItem("Minjung/mapeditor")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(MapGeneratorEditor));
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField ("This is label.");
		if (GUILayout.Button("sample button."))
        {
            TestFieldTiles tileGenerator = GameObject.FindObjectOfType<TestFieldTiles> ();
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
