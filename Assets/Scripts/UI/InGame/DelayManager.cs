using UnityEngine;
using System.Collections;

public class DelayManager : MonoBehaviour
{
	// unity unit per time.
	public float playerMoveSpeed = 5f;

	public static DelayManager instance;

	public static DelayManager Get()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
	}
}
