using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

	public Sprite upLeftArrow;
	public Sprite midLeftArrow;
	public Sprite downLeftArrow;
	public Sprite upRightArrow;
	public Sprite midRightArrow;
	public Sprite downRightArrow;

	private static TileManager.TileDirection arrowDirection;

	public static void SetArrowDirection (TileManager.TileDirection enumFormTileKey)
	{
		arrowDirection = enumFormTileKey;
	}

	public TileManager.TileDirection GetArrowDirection ()
	{
		return arrowDirection;
	}

	public void SetArrowImage()
	{
		Debug.Log("Set Arrow Img");
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

		if (arrowDirection == TileManager.TileDirection.UpLeft)
		{
			spriteRenderer.sprite = upLeftArrow;
		}
		else if (arrowDirection == TileManager.TileDirection.MidLeft)
		{
			spriteRenderer.sprite = midLeftArrow;
		}
		else if (arrowDirection == TileManager.TileDirection.DownLeft)
		{
			spriteRenderer.sprite = downLeftArrow;
		}
		else if (arrowDirection == TileManager.TileDirection.UpRight)
		{
			spriteRenderer.sprite = upRightArrow;
		}
		else if (arrowDirection == TileManager.TileDirection.MidRight)
		{
			spriteRenderer.sprite = midRightArrow;
		}
		else if (arrowDirection == TileManager.TileDirection.DownRight)
		{
			spriteRenderer.sprite = downRightArrow;
		}
	}

	void Awake () 
	{
		SetArrowImage();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown()
	{
		//characterManagerInstance.selectedDirection = GetArrowDirection();
		//characterManagerInstance.DestroyAllDirectionArrows();
	}
}
