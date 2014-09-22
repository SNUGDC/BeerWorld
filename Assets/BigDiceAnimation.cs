using UnityEngine;
using System.Collections;

public class BigDiceAnimation : MonoBehaviour {
	Animator anim;
	int resultNum;
	public delegate void DiceGetter(int diceResult);
	public DiceGetter diceGetter;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	public void setNumber(int num){
		anim.SetInteger("num",num);
		anim.SetTrigger("pop");
	}
	public void reset(){
		anim.SetTrigger ("reset");
	}

	void OnMouseDown(){
		MultiAudioClip multiAudioClip = GetComponent<MultiAudioClip>();
		multiAudioClip.audioSources[0].playOnAwake=false;
		multiAudioClip.audioSources[0].loop = true;
		multiAudioClip.audioSources[0].Play ();
	}
	void OnMouseUp(){
		StartCoroutine("ShowRandomDice");
	}
	IEnumerator ShowRandomDice(){
		anim.SetTrigger("stopRoll");
		MultiAudioClip multiAudioClip = GetComponent<MultiAudioClip>();
		for(int j=1; j<4; j++){
			for(int i=0; i<4; i++){
				multiAudioClip.audioSources[0].loop=false;
				multiAudioClip.audioSources[0].Play ();
				int num = (int)(Random.value*6)+1;
				setNumber(num);
				yield return new WaitForSeconds(0.1f*j);
			}
		}
		multiAudioClip.audioSources[1].playOnAwake=false;
		multiAudioClip.audioSources [1].Play ();
		resultNum = (int)(Random.value*6)+1;
		setNumber(resultNum);
		if(diceGetter.Method!=null)
			diceGetter(resultNum);
		
	}
}
