using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {

	private static TileManager singletonInstance = null;

	public static Tile GetStartTile()
	{
		foreach (var tile in singletonInstance.tileList)
        {
            if (tile.IsStartTile())
            {
                return tile;
            }
        }

        Debug.LogError("Cannot find start tile.");
        return null;
	}


	List<Tile> tileList = new List<Tile>();

    Dictionary<int, Tile> tileDictionary = new Dictionary<int, Tile>();

	void Awake () {

		singletonInstance = this;

        Tile[] tiles = FindObjectsOfType(typeof(Tile)) as Tile[];
        foreach (Tile tile in tiles)
        {
            tileList.Add(tile);
            Vector2 tilePreCoordinate = FieldTileUtility.getTranslatedCoordinate(tile.transform.localPosition.x, tile.transform.localPosition.y);
			Vector2 tileCoordinate = new Vector2(tilePreCoordinate.x + 1, tilePreCoordinate.y + 1); 
            int tileKey = (int)(tileCoordinate.x * 100 + tileCoordinate.y);
            Debug.Log(tile.gameObject.name + " : " + tileCoordinate.x + ", " + tileCoordinate.y);
            tileDictionary.Add(tileKey, tile);
        }

        Debug.Log("length of List : " + tileList.Count);
        Debug.Log("length of Dictionary : " + tileDictionary.Count);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
