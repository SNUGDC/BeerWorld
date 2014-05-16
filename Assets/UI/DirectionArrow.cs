using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

	public Sprite upLeftArrow;
	public Sprite midLeftArrow;
	public Sprite downLeftArrow;
	public Sprite upRightArrow;
	public Sprite midRightArrow;
	public Sprite downRightArrow;

	private TileManager.TileDirection arrowDirection;

	public static void SetArrowDirection (TileManager.TileDirection enumFormTileKey)
	{
		arrowDirection = enumFormTileKey;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{

	}
}
