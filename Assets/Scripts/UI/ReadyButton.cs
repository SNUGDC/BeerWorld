using UnityEngine;
using System.Collections;

public class ReadyButton : MonoBehaviour {
	MyCharacter myChar;
	bool clicked = false;

	// Use this for initialization
	void Start () {
		myChar = FindObjectOfType<MyCharacter> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseUp(){
		clicked = !clicked;
		myChar.GetReady (clicked);
		if(clicked)
			GameObject.Find ("Button").GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/mainUI/components/lobby_ready_touch", typeof(Sprite)) as Sprite;
		else
			GameObject.Find ("Button").GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/mainUI/components/lobby_ready", typeof(Sprite)) as Sprite;
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<BoxCollider2D> ().enabled = true;
	}

	void OnMouseEnter(){
		Debug.Log ("enter");
	}

	void OnMouseExit(){
		Debug.Log ("OUT");
	}
}
