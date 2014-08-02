using UnityEngine;
using System.Collections;

public class DevTestStart : MonoBehaviour
{
	void Start()
	{
		Network.InitializeServer(3, 25008, false);
	}

	void OnServerInitialized()
	{
		Application.LoadLevel("Battle");
	}
}
