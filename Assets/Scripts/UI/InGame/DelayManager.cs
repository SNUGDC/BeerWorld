using UnityEngine;
using System.Collections;

public class DelayManager : MonoBehaviour
{
	// unity unit per time.
	public float playerMoveSpeed = 5f;
	public float battleDiceResultToAttackDelay = 0.2f;
	public float battleHpMinusDelay = 0.3f;
	public float battleDiceRollToDiceResultDelay = 0.2f;

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