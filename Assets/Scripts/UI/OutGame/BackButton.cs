using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {
	RoomManager rmManager;

	// Use this for initialization
	void Start () {
		rmManager = FindObjectOfType<RoomManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp(){
		rmManager.DisconnectFromServer ();
		audio.Play ();
	}
}
