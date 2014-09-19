using UnityEngine;
using System.Collections;

public class ItemSlotAnimation : MonoBehaviour {
	private Animator anim;
	void Awake(){
		anim = GetComponent<Animator>();
	}
	
	void PopInventory(bool boolean){
		anim.SetBool("Active",boolean);
	}
}
