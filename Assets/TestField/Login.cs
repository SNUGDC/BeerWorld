using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	string username="";
	float height_unit = Screen.height / 720f * 1.4f;
	float width_unit =  Screen.width / 1280f * 1.4f;
	public GUISkin guiskin;

	// Use this for initialization
	void Start () {
		height_unit = Screen.height / 720f;
		guiskin.textField.fontSize = (int) (20 * height_unit);
		guiskin.label.fontSize = (int) (20 * height_unit);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = guiskin;
		username = GUI.TextField(new Rect(10*width_unit,10*height_unit,150*width_unit,40*height_unit),username);
		PlayerPrefs.SetString ("id", username);
		if(GUI.Button(new Rect(10*width_unit,60*height_unit,150*width_unit,50*height_unit),"Login")){
			//username = "";
			Application.LoadLevel("Lobby");
		}
	}
}
