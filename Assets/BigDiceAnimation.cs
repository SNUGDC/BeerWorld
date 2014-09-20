using UnityEngine;
using System.Collections;

public class BigDiceAnimation : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	public void setNumber(int num){
		anim.SetInteger("num",num);
		anim.SetTrigger("pop");
	}
	public void roll(){
		anim.SetTrigger("roll");
	}
	public void reset(){
		anim.SetTrigger ("reset");
	}

	void OnMouseDown(){
		roll();
	}
	void OnMouseUp(){
		StartCoroutine("ShowRandomDice");
	}
	IEnumerator ShowRandomDice(){
		for(int j=1; j<4; j++){
			for(int i=0; i<4; i++){
				int num = (int)(Random.value*6)+1;
				setNumber(num);
				yield return new WaitForSeconds(0.1f*j);
			}
		}
		setNumber((int)(Random.value*6)+1);
	}
}
