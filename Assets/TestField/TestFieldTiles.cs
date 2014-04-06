using UnityEngine;
using System.Collections;

public class TestFieldTiles : MonoBehaviour {

	public Tile tile;

	const int MapWidth = 15;
	const int MapHeight = 15;

	public void GenerateTile ()
	{
		for (int i = 0; i < MapWidth; i++) {
			for (int j = 0; j < MapHeight; j++) {
				Vector2 tilePosition;
				tilePosition = TestFieldManager.getTranslatedPosition (i, j);

//				float halfWidth = TestFieldManager.unityTileX * (MapWidth-1) / 2;
//				float halfHeight = (TestFieldManager.unityTileY * (MapHeight-1) - (0.15f * (MapHeight-2))) / 2;
				Vector3 zerozeroPosition = new Vector3 (tilePosition.x, tilePosition.y, j*0.1f);
				Tile tileInstance = Instantiate (tile, zerozeroPosition, Quaternion.identity) as Tile;

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
