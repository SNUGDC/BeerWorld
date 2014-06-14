using UnityEngine;
using System.Collections;

public class DiceRollerButton : MonoBehaviour {

    public CharacterManager characterManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		CharacterManager.MoveState moveState = CharacterManager.GetMoveState();
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

    void OnMouseDown() {
        int diceResult = Dice.Roll();
        Debug.Log("Dice : " + diceResult);
        characterManager.SetMovement(diceResult);
    }
}
