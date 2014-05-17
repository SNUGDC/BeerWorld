using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public enum TileType
	{
        Default, Start, Item, Buff, Warp, Jail, Save		
    }

	public bool isVisible;
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

	// Use this for initialization
	void Start () {
//		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
