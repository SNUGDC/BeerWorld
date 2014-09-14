﻿using UnityEngine;
using System.Collections;

public class LoginOrSignButton : MonoBehaviour {
	string username="admin";
	float height_unit = Screen.height / 720f * 1.4f;
	float width_unit =  Screen.width / 1280f * 1.4f;
	string url = "http://147.46.76.106:8000/";
	string response;

	// Use this for initialization
	void Start () {
		username = SystemInfo.deviceUniqueIdentifier;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		StartCoroutine( Request2Web("login"));
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
