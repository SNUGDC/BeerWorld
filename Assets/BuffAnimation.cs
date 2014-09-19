using UnityEngine;
using System.Collections;

public class BuffAnimation : MonoBehaviour {
	public GameObject[] BuffIcons;
	private Camera mainCam;
	private Camera uiCam;

	void Awake(){
		mainCam = Camera.main;
		uiCam = GameObject.FindWithTag("UICamera").camera;
	}

	public void PlayBuffAt(Vector3 tilePos, int iconNum){
		Vector3 translatedPos;
		translatedPos = uiCam.ScreenToWorldPoint(mainCam.WorldToScreenPoint(tilePos));
		iTween.MoveFrom(BuffIcons[iconNum],iTween.Hash("position",translatedPos,"easetype",iTween.EaseType.easeInCubic,"time",0.5f));
		iTween.ScaleFrom(BuffIcons[iconNum],iTween.Hash("scale",Vector3.one*3f,"easetype",iTween.EaseType.easeInCubic,"time",0.5f));
	}
}
