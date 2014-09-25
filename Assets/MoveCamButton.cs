using UnityEngine;
using System.Collections;

public class MoveCamButton : MonoBehaviour {
	public Vector3 moveTarget;

	void OnMouseUp(){
		iTween.MoveTo(Camera.main.gameObject,iTween.Hash("easetype",iTween.EaseType.easeOutCubic,"position",moveTarget,"time",0.5f));
	}
}
