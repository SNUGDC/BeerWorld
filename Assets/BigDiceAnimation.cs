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
}
