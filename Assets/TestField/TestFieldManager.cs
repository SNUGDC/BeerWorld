using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestFieldManager : MonoBehaviour {

	public int TW = 58;
	public int TH = 64;

	public Vector2 getTranslatedPosition(float i, float j){
		float posX, posY;
		if (j%2 == 1){
			posX = 77+TW/2+TW*(i-1);
			posY = 4+TH/2+(TH-15)*(j-1);
		}
		else {
			posX = 77+TW*(i-1);
			posY = 4+TH/2+(TH-15)*(j-1);
		}		
		return new Vector2(posX, posY);
	}
							
	public Vector2 getTranslatedCoordinate(float x, float y){
		float i, j;	
		j = math.floor((y-11.5)/(TH-15)) +1;
		if (j%2 == 1){
			i = math.floor(((x-77)/TW)+1); 
		}
		else {
			i = math.floor(((x-77)/TW)+3/2);
		}
		return new Vector2(i, j);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
