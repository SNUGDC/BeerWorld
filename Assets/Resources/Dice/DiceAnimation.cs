using UnityEngine;
using System.Collections;

public class DiceAnimation : MonoBehaviour {
	static int rollState = Animator.StringToHash("Base.roll");
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

	public bool IsRollAnimating()
	{
		var currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		return currentBaseState.nameHash == rollState;
	}
}
