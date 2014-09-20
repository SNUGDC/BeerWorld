using UnityEngine;
using System.Collections;

public class MyCharacter : MonoBehaviour {
	string m_currCharacter = "Warrior";
	SpriteRenderer spRender;
	bool ready;
	int myID;

	// Use this for initialization
	void Start () {
		spRender = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	string currCharacter{ 
		get{ 
			return m_currCharacter;
		} 
	}

	public void changeCharacter(int move){
		int currState = (int) Character.CharClass.Parse (typeof(Character.CharClass),m_currCharacter) + move;
		if(currState < 1){
			currState = 3;
		}
		else if(currState > 3){
			currState = 1;
		}
		m_currCharacter = ((Character.CharClass) currState).ToString();
		Sprite sprite = Resources.Load("Images/mainUI/components/lobby_char_"+m_currCharacter,typeof(Sprite)) as Sprite;
		spRender.sprite = sprite;
	}

	public void Ready(bool _ready){
		ready = _ready;
	}
}
