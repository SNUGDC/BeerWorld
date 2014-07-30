using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	// Called by send message.
	void OnCmaeraFollow()
	{
		Camera.main.transform.position = new Vector3(
			transform.position.x,
			transform.position.y,
			Camera.main.transform.position.z);
	}
}
