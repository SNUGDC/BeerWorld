using UnityEngine;
using UnityEditor;
using System.Collections;

public class TileUtilityHelperWindow : EditorWindow {

    [MenuItem("Test/MapUtil")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TileUtilityHelperWindow));
    }

    KeyToPositionShower keyToPositionShower = null;

    void OnEnable()
    {
        keyToPositionShower = new KeyToPositionShower();
    }

    private GameObject tileManager;

    void OnGUI()
    {

        if (GUILayout.Button("Move TIle to upright."))
        {
            var tileManager = FindObjectOfType(typeof(TileManager)) as TileManager;

            foreach(Transform tile in tileManager.transform)
            {
                //tile.transform.position = 
                //    tile.transform.position +
                //    new Vector3(0.29f, 0);
            }
        }

        keyToPositionShower.Show();
        EditorGUILayout.Space();
        ShowPositionToKey();
    }

    private Vector2 position = Vector2.zero;
    private int coordX = 0;
    private int coordY = 0;
    void ShowPositionToKey()
    {
        EditorGUILayout.LabelField("position to tile key.");
        position = EditorGUILayout.Vector2Field("position", position);

        if (GUILayout.Button("Calculate"))
        {
            Vector2 coordXY = FieldTileUtility.GetTranslatedCoordinate(position.x, position.y);
            coordX = (int)coordXY.x;
            coordY = (int)coordXY.y;
        }
        coordX = EditorGUILayout.IntField("coord x", coordX);
        coordY = EditorGUILayout.IntField("coord y", coordY);
    }

    class KeyToPositionShower
    {
        private Vector2 positionToShow = Vector2.zero;
        private int coordX = 0;
        private int coordY = 0;

        public void Show()
        {
            EditorGUILayout.LabelField("tile key to position");
            coordX = EditorGUILayout.IntField("coord x", coordX);
            coordY = EditorGUILayout.IntField("coord y", coordY);

            if (GUILayout.Button("Calculate"))
            {
                positionToShow = FieldTileUtility.GetTranslatedPosition(coordX, coordY);
            }

            EditorGUILayout.Vector2Field("position", positionToShow);
        }
    }
}
