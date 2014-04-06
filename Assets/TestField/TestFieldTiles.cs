using UnityEngine;
using System.Collections;

public class TestFieldTiles : MonoBehaviour {

	public Tile tile;

	public void GenerateTile ()
	{
		for (int i = 0; i < 12; i++) {
			for (int j = 0; j < 13; j++) {
				Vector2 tilePosition;
				tilePosition = TestFieldManager.getTranslatedPosition (i, j);
				Tile tileInstance = Instantiate (tile, new Vector3 (tilePosition.x, tilePosition.y, j*0.1f), Quaternion.identity) as Tile;

				GameObject tileInstanceGameObject = tileInstance.gameObject;
				GameObject testFieldTilesGameObject = gameObject;

				Transform tileInstanceTransform = tileInstanceGameObject.GetComponent<Transform>();
				Transform tileTestTilesTransform = testFieldTilesGameObject.GetComponent<Transform>();

				tileInstanceTransform.parent = testFieldTilesGameObject.transform;
			}
		}
	}

	// Use this for initialization
	void Start () 
    {
        GenerateTile ();
    }

}
