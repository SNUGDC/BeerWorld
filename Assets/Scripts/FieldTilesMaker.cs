using UnityEngine;
using System.Collections;

public class FieldTilesMaker : MonoBehaviour {

	public Tile tile;

	const int MapWidth = 15;
	const int MapHeight = 15;

	public void GenerateTile ()
	{
		for (int i = 0; i < MapWidth; i++) {
			for (int j = 0; j < MapHeight; j++) {
				Vector2 tilePosition;
				tilePosition = FieldTileUtility.GetTranslatedPosition (i, j);

				Vector3 zerozeroPosition = new Vector3 (tilePosition.x, tilePosition.y, j*0.1f);
				Tile tileInstance = Instantiate (tile, zerozeroPosition, Quaternion.identity) as Tile;

				GameObject tileInstanceGameObject = tileInstance.gameObject;
				GameObject testFieldTilesGameObject = gameObject;

				Transform tileInstanceTransform = tileInstanceGameObject.GetComponent<Transform>();
				Transform tileTestTilesTransform = testFieldTilesGameObject.GetComponent<Transform>();

                tileInstanceGameObject.name = "(" + i + "," + j + ") tile";
				tileInstanceTransform.parent = testFieldTilesGameObject.transform;
			}
		}
	}

	// Use this for initialization
	void Start () 
    {
      //  GenerateTile ();
    }
}
