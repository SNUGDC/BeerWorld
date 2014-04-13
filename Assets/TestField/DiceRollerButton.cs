using UnityEngine;
using System.Collections;

public class DiceRollerButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    //Instantiate(
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        Debug.Log("Dice : " + Dice.Roll());
    }
}
