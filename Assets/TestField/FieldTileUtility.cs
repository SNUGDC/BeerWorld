﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldTileUtility : MonoBehaviour {

	public static float TilePixelWidth = 58;
	public static float TilePixelHeight = 64;
    public static float unityTileX = TilePixelWidth / 100;
    public static float unityTileY = TilePixelHeight / 100;

	public static Vector2 GetTranslatedPosition(float i, float j){
		float posX, posY;
		float zeroIndexX = i - 1;
		float zeroIndexY = j - 1;
		if (j%2 == 1){
			posX = unityTileX/2+unityTileX * zeroIndexX;
			posY = unityTileY/2+(unityTileY-0.15f) * zeroIndexY;
		}
		else {
			posX = unityTileX * zeroIndexX;
			posY = unityTileY/2+(unityTileY-0.15f) * zeroIndexY;
		}		
		return new Vector2(posX, posY);
	}
							
	public static Vector2 GetTranslatedCoordinate(float x, float y){
		float i, j;
		
		j = (int)((((y - unityTileY / 2) / (unityTileY - 0.15f)) + 1) + 0.1f) +1;
		i = (int)((x / unityTileX + 1) + 0.4f) +1;
		
		return new Vector2(i, j);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
