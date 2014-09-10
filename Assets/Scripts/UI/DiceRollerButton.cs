using UnityEngine;
using System.Collections.Generic;

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

    List<BDice.Species> GetMoveDices(Character player, List<BDice.Species> moveDices)
    {
        for (int i = 0; i < player.numberOfMoveDice; i++)
        {
            moveDices.Add(player.speciesOfMoveDice);
        }
        return moveDices;
    }

    void OnMouseDown()
    {
        List<BDice.Species> moveDices = new List<BDice.Species>();
        moveDices = GetMoveDices(characterManager.GetCharacterInstance(), moveDices);
        int diceResult = 0;
        for (int i = 0; i < moveDices.Count; i++)
        {
            diceResult += Dice.Roll(moveDices [i]);
        }
        Debug.Log("Dice : " + diceResult);

        characterManager.SetMovement(diceResult);
    }
}
