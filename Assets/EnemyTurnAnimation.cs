using UnityEngine;
using System.Collections;

public class EnemyTurnAnimation : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponent<Animator>();
	}
	
	void turnPlayer(){
		anim.SetTrigger("TurnPlayer");
	}
	void turnEnemy(){
		anim.SetTrigger("TurnEnemy");
	}
	void engaged(){
		anim.SetTrigger("Engaged");
	}
}
