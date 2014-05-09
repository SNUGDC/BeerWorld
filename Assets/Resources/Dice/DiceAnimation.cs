using UnityEngine;
using System.Collections;

public class DiceAnimation : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void rollByNumber(int num){
		anim.SetInteger("num",num);
		anim.SetTrigger("roll");
	}
}
