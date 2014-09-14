using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	string username="";
	float height_unit = Screen.height / 720f * 1.4f;
	float width_unit =  Screen.width / 1280f * 1.4f;
	string url = "http://147.46.76.106:8000/";
	string response;
	bool can_login = false;
	bool sign_in = false;
	public GUISkin guiskin;

	// Use this for initialization
	void Start () {
		height_unit = Screen.height / 720f;
		guiskin.textField.fontSize = (int) (20 * height_unit);
		guiskin.label.fontSize = (int) (20 * height_unit);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(can_login){
			can_login = false;
			StartCoroutine(Request2Web("login"));
			//Application.LoadLevel("Lobby");
		}
		if(sign_in){
			sign_in = false;
			StartCoroutine(Request2Web("signin"));
		}
	
	}

	void OnGUI(){
		GUI.skin = guiskin;
		username = GUI.TextField(new Rect(10*width_unit,10*height_unit,150*width_unit,40*height_unit),username);
		PlayerPrefs.SetString ("id", username);
		if(GUI.Button(new Rect(10*width_unit,60*height_unit,150*width_unit,50*height_unit),"Login")){
			can_login = true;
			//StartCoroutine(Login2Web());

			//username = "";
		}
		if(GUI.Button (new Rect(10*width_unit,115*height_unit,150*width_unit,50*height_unit),"Sign In")){
			sign_in = true;
		}
	}

	IEnumerator Request2Web(string req){
		Debug.Log ("Start " + req + "... : " + username);
		WWWForm webform = new WWWForm();
		webform.AddField("id",username);
		WWW webReq = new WWW(url + req, webform);
		yield return webReq;

		Debug.Log ("Finished " + req + "... : " + webReq);
		response = webReq.text;
		if(response == "OK"){
			Application.LoadLevel("Lobby");
		}
		else{
			StartCoroutine(Request2Web("signin"));
		}
	}

}
