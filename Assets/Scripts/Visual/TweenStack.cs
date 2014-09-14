using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenStack : MonoBehaviour {
	List<Hashtable> hash = new List<Hashtable>();

	public void Add(Hashtable newHash){
		hash.Add(newHash);
		if(hash.Count==1)
			nextTween();
	}

	void nextTween(){
		if(hash.Count>0){
			hash[0].Add("oncomplete","nextTween");
			hash[0].Add("oncompletetarget",gameObject);
			iTween.MoveTo (gameObject,hash[0]);
			hash.RemoveAt(0);
		}
	}
}
