﻿using UnityEngine;
using System.Collections;

public class GameStartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Network.isClient == true)
		{
			gameObject.SetActive(false);
		}
	}

	void OnMouseDown ()
	{
		GameManager.gameManagerInstance.GameStart();

		NetworkManager.SendUsersNetworkViewID();
		gameObject.SetActive(false);
	}
}
