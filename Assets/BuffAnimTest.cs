using UnityEngine;
using System.Collections;

public class BuffAnimTest : MonoBehaviour {
	private BuffAnimation buffAnim;
	// Use this for initialization
	void Awake () {
		buffAnim = FindObjectOfType<BuffAnimation>();
	}

	void OnMouseDown(){
		buffAnim.PlayBuffAt(transform.position,0);
	}
}
