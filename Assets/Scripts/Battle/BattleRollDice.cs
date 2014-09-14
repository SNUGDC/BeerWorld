using System;
using UnityEngine;
using System.Collections;

public class BattleRollDice : MonoBehaviour
{
	public BattleManager battleManager;

	void OnMouseDown()
	{
		NetworkManager.BattleRoleDice();
	}

	void On()
	{
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = true;
		collider2D.enabled = true;
	}

	void Off()
	{
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
		collider2D.enabled = false;
	}

	void Update()
	{
		if (!BattleManager.battleManagerInstance.isMine) {
			Off();
			return;
		}

		BattleManager.State battleState = BattleManager.battleManagerInstance.GetBattleState();

		if (battleState == BattleManager.State.WaitingRoll)
		{
			On();
		}
		else
		{
			Off();
		}
	}
}
