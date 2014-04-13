using UnityEngine;
using System.Collections;

public class CharManager : MonoBehaviour {

	public Character characterPrefeb;

	// Use this for initialization
	void Start () {
		Tile startTile = TileManager.GetStartTile ();
        Vector3 startPosition = new Vector3(startTile.gameObject.transform.position.x, startTile.gameObject.transform.position.y, Character.Depth);
        Character characterInstance = Instantiate(characterPrefeb, startPosition, Quaternion.identity) as Character; 	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
