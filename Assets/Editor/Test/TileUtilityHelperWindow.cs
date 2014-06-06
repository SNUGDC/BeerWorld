using UnityEngine;
using UnityEditor;
using System.Collections;

public class TileUtilityHelperWindow : EditorWindow {

    [MenuItem("Test/MapUtil")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TileUtilityHelperWindow));
    }

    private Vector2 positionToShow = Vector2.zero;
    private int coordX = 0;
    private int coordY = 0;

    void OnGUI()
    {

        EditorGUILayout.LabelField("tile key to position");
        coordX = EditorGUILayout.IntField("x", coordX);
        coordY = EditorGUILayout.IntField("y", coordY);

        if (GUILayout.Button("Calculate"))
        {
            positionToShow = FieldTileUtility.GetTranslatedPosition(coordX, coordY);
        }

        EditorGUILayout.Vector2Field("coordinate", positionToShow);
    }
}
