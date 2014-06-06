using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour {

	public Sprite upLeftArrow;
	public Sprite midLeftArrow;
	public Sprite downLeftArrow;
	public Sprite upRightArrow;
	public Sprite midRightArrow;
	public Sprite downRightArrow;

	public bool setDirection;

	private TileManager.TileDirection arrowDirection;

	public void SetArrowDirection (TileManager.TileDirection enumFormTileKey)
	{
		arrowDirection = enumFormTileKey;
	}

	public TileManager.TileDirection GetArrowDirection ()
	{
		return arrowDirection;
	}

	public void SetArrowImage()
	{
		Debug.Log("Set Arrow Img, ArrowDirection : " + arrowDirection);
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
	//	SetArrowImage();
	}

	// Use this for initialization
	void Start () {
		setDirection = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (setDirection == false)
		{
			SetArrowImage();
			setDirection = true;
		}
	}

	void OnMouseDown()
	{
		//characterManagerInstance.selectedDirection = GetArrowDirection();
		//characterManagerInstance.DestroyAllDirectionArrows();
	}
}
