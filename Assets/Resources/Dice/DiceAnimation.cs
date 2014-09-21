using UnityEngine;
using System.Collections;

public class DiceAnimation : MonoBehaviour {
	static int rollState = Animator.StringToHash("Base.roll");
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	public void rollByNumber(int num){
		anim.SetBool("is4",false);
		anim.SetInteger("num",num);
		anim.SetTrigger("roll");
	}

	public void roll4ByNumber(int num){
		anim.SetBool("is4",true);
		anim.SetInteger("num",num);
		anim.SetTrigger("roll");
	}

	public bool IsRollAnimating()
	{
		var currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		return currentBaseState.nameHash == rollState;
	}
}
