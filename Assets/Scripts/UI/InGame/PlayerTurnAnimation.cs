using UnityEngine;
using System.Collections;

public class PlayerTurnAnimation : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponent<Animator>();
	}
	
	public void SetTurn(int turn){
		anim.SetInteger("Turn",turn);
	}
}
