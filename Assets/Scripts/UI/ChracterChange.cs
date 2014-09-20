using UnityEngine;
using System.Collections;

public class ChracterChange : MonoBehaviour {
	public string direction;
	GameObject character;
	MyCharacter myCharacter;

	// Use this for initialization
	void Start () {
		myCharacter = GameObject.Find("myCharacter").GetComponent<MyCharacter> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp(){
		Debug.Log (myCharacter);
		int dir = 0;
		if(direction == "left")
			dir = -1;
		else if (direction == "right")
			dir = 1;
		myCharacter.changeCharacter (dir);
	}
}
