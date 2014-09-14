using UnityEngine;
using System.Collections;

public class DevTestStart : MonoBehaviour
{
	public string sceneName = "Battle";

	void Start()
	{
		Network.InitializeServer(3, 25008, false);
	}

	void OnServerInitialized()
	{
		Application.LoadLevel(sceneName);
	}
}
