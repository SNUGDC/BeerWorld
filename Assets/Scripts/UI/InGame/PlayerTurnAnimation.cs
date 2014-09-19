using UnityEngine;
using System.Collections;

public class PlayerTurnAnimation : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponent<Animator>();
	}
	
	void SetTurn(int turn){
		anim.SetInteger("Turn",turn);
	}
}
