using UnityEngine;
using System.Collections;

public class TestFieldTiles : MonoBehaviour {

	public Tile tile;

	// Use this for initialization
	void Start () 
    {
        for (int i=0; i<12; i++)
        {
            for (int j=0; j<13; j++)
            {
                Vector2 tilePosition;
                tilePosition = TestFieldManager.getTranslatedPosition(i, j);
                Instantiate(tile, new Vector3(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
            }
        }			           
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
