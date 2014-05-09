using UnityEngine;
using System.Collections;

public class TestDice : MonoBehaviour {
	DiceAnimation diceCon;
	int diceNum = 5;
	// Use this for initialization
	void Start () {
		diceCon = GetComponent<DiceAnimation>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Debug.Log("Click");
			diceCon.rollByNumber(diceNum);
			switch(diceNum){
			case 5:
				diceNum=2;
				break;
			case 2:
				diceNum=4;
				break;
			case 4:
				diceNum=5;
				break;
			}
		}
	}

}
