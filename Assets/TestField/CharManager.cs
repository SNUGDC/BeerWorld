using UnityEngine;
using System.Collections.Generic;

public class CharManager : MonoBehaviour {

	public Character characterPrefeb;
    private Character characterInstance;
    public int howManyMove = 0;

    public Dictionary<TileManager.TileDirection, Tile> borderDictionary = new Dictionary<TileManager.TileDirection, Tile>();
        

    void SearchBorderTiles () 
    {
        Vector3 position = characterInstance.transform.position;
        Vector2 characterCoordinate = FieldTileUtility.GetTranslatedCoordinate(position.x, position.y);
        Debug.Log("characterPos : (" + position.x + ", " + position.y + ")");
        Debug.Log("characterCoord : (" + characterCoordinate.x + ", " + characterCoordinate.y + ")");
        borderDictionary = TileManager.GetTileDictionaryOfBorderTiles(characterCoordinate);
    }

    void MoveCharacter () {
        //bool movable = true;
        SearchBorderTiles();
        foreach (KeyValuePair<TileManager.TileDirection, Tile> pair in borderDictionary)
        {
            if (pair.Value != null)
            {
//                Vector2 nextTilePosition = new Vector2(pair.Value.transform.position.x, pair.Value.transform.position.y);
//                if (nextTilePosition != characterPrefeb.prePosition)
//                {
//                    if (movable == true)
//                    {
//                        characterPrefeb.prePosition = new Vector2(characterPrefeb.transform.position.x, characterPrefeb.transform.position.y);
//                        Vector2 nextPosition = FieldTileUtility.GetTranslatedPosition((int)pair.Key/100, (int)pair.Key%10);
//                        characterPrefeb.transform.position = new Vector3(nextPosition.x, nextPosition.y, Character.Depth);
//                        Vector2 newCoordinate = FieldTileUtility.GetTranslatedCoordinate(nextPosition.x, nextPosition.y);
//                        characterPrefeb.position = new Vector2(characterPrefeb.transform.position.x, characterPrefeb.transform.position.y);
//
//                        movable = false;
//                    }
//                }
                  Debug.Log("Key : " + pair.Key + " , Value : " + pair.Value);
            }
        }
    }

	// Use this for initialization
	void Start () {
		Tile startTile = TileManager.GetStartTile ();
        Vector3 startTilePosition = startTile.gameObject.transform.position;
        Vector3 startPositionOfCharacter = new Vector3(startTilePosition.x, startTilePosition.y, Character.Depth);
        characterInstance = Instantiate(characterPrefeb, startPositionOfCharacter, Quaternion.identity) as Character;
//        characterPrefeb.position = new Vector2(characterPrefeb.transform.position.x, characterPrefeb.transform.position.y); 
	}
	
	// Update is called once per frame
	void Update () {
        while (howManyMove > 0)
        {
            MoveCharacter ();
            howManyMove--;
        }
	}
}
