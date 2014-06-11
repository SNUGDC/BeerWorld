using UnityEngine;
using System.Collections;

public class DiceRollerButton : MonoBehaviour {

    public CharacterManager characterManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        int diceResult = Dice.Roll();
        Debug.Log("Dice : " + diceResult);
        characterManager.SetMovement(diceResult);
    }
}
