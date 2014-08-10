using UnityEngine;
using System.Collections;

public class BattleRollDice : MonoBehaviour
{
	public BattleManager battleManager;

	void OnMouseDown()
	{
		battleManager.OnRollClicked();
	}

	void Update()
	{
		BattleManager.State battleState = BattleManager.battleManagerInstance.GetBattleState();
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

		if (battleState == BattleManager.State.WaitingRoll)
		{
			spriteRenderer.enabled = true;
			collider2D.enabled = true;
		}		
		else
		{
			spriteRenderer.enabled = false;
			collider2D.enabled = false;
		}
	}
}
