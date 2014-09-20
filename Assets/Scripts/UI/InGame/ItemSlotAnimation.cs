using UnityEngine;
using System.Collections;

public class ItemSlotAnimation : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponentInChildren<Animator>();
	}
	
	void itemCountActive(bool boolean){
		anim.SetBool("Active",boolean);
	}
}
