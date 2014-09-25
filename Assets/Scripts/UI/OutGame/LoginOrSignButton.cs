using UnityEngine;
using System.Collections;

public class LoginOrSignButton : MonoBehaviour {
	string username="admin";
	float height_unit = Screen.height / 720f * 1.4f;
	float width_unit =  Screen.width / 1280f * 1.4f;
	string url = "http://147.46.241.250:8000/";
	string response;
	SpriteRenderer spRender;
	public Sprite idleButton;
	public Sprite pushedButton;
	public GameObject loadingPop;

	// Use this for initialization
	void Start () {
		username = SystemInfo.deviceUniqueIdentifier;
		idleButton = Resources.Load ("Images/mainUI/components/login_idle", typeof(Sprite)) as Sprite;
		pushedButton = Resources.Load ("Images/mainUI/components/login_touch", typeof(Sprite)) as Sprite;
		spRender = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log (pushedButton);
		spRender.sprite = pushedButton;
		audio.Play ();
	}

	void OnMouseUp(){
		if(loadingPop != null)
			loadingPop.SetActive(true);
		StartCoroutine( Request2Web("login"));
		spRender.sprite = idleButton;
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
