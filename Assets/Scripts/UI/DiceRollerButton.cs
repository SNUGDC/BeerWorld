using UnityEngine;
using System.Collections;

public class DiceRollerButton : MonoBehaviour
{	
	CharacterManager characterManager = null;
	void Start()
	{
		characterManager  = GameManager.GetMyCharacterManager();
	}
	// Update is called once per frame
	void Update ()
	{		
		CharacterManager.MoveState moveState = characterManager.GetMoveState();
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if (moveState == CharacterManager.MoveState.Idle)
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

    void OnMouseDown()
    {
        int diceResult = Dice.Roll();
        Debug.Log("Dice : " + diceResult);

        characterManager.SetMovement(diceResult);
    }
}
