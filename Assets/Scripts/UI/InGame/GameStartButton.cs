using UnityEngine;
using System.Collections;

public class GameStartButton : MonoBehaviour {
	void Update ()
	{
		if (Network.isClient == true)
		{
			gameObject.SetActive(false);
		}
		else if (Network.connections.Length > 0)
		{
			gameObject.SetActive(false);
		}
	}

	void OnMouseDown ()
	{
		GameManager.gameManagerInstance.GameStart();
		gameObject.SetActive(false);
	}
}
