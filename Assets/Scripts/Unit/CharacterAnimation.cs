﻿using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponentInChildren<Animator>();
	}

	void Move(){
		anim.SetTrigger("Move");
	}
	void Attack(){
		anim.SetTrigger("Atk");
	}
	void Hit(){
		anim.SetTrigger("Hit");
	}
}
