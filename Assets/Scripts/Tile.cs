using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public enum TileType
	{
        Default, Start, Item, Buff, Warp, Jail, Save		
    }

	public bool isVisible;
    [SerializeField]
    private int tileKey = 0;
	//public bool isStartTile;

    public TileType tileType;

	public Sprite defaultTile;
	public Sprite startTile;
	public Sprite itemTile;
	public Sprite buffTile;
	public Sprite warpTile;
	public Sprite jailTile;
	public Sprite saveTile;

    public bool IsStartTile()
    {
        return tileType == TileType.Start;
    }

    public void SetTileKey(int key)
    {
        tileKey = key;
    }

    public Vector2 GetCoord()
    {
        return FieldTileUtility.GetCoordFromKey(tileKey);
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
