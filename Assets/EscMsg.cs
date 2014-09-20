using UnityEngine;
using System.Collections;

public class EscMsg : MonoBehaviour {
	private Animator anim;
	private TextMesh txt;
	public string[] messages;
	void Awake(){
		anim = GetComponent<Animator>();
		txt = GetComponent<TextMesh>();
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			anim.SetTrigger("PopMsg");
			txt.text = messages[(int)(messages.Length * Random.value)];
		}
	}
}
