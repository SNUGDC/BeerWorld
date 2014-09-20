using UnityEngine;
using System.Collections.Generic;

public class DiceRollerButton : MonoBehaviour
{	
	public GameObject BigDice;
	private int result;
	void Update ()
	{		
		var characterManager = GameManager.GetMyCharacterManager();
		if (characterManager == null)
		{
			return;
		}

		CharacterManager.MoveState moveState = characterManager.GetMoveState();
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if (moveState == CharacterManager.MoveState.Idle)
		{
			BigDice.SetActive(true);
			BigDice.GetComponent<BigDiceAnimation>().diceGetter = getDice;
//			spriteRenderer.enabled = true;
//			collider2D.enabled = true;
//		}		
//		else
//		{
//			spriteRenderer.enabled = false;
//			collider2D.enabled = false;
		}

		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			characterManager.SetMovement(1);
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

	void getDice(int diceResult){
		result = diceResult;
		Invoke("setOffDice",0.5f);
	}

	void setOffDice(){
		BigDice.SetActive(false);
		var characterManager = GameManager.GetMyCharacterManager();
		characterManager.SetMovement(result);
	}

    void OnMouseDown()
    {
			var characterManager = GameManager.GetMyCharacterManager();
        List<BDice.Species> moveDices = new List<BDice.Species>();
		audio.Play();
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
