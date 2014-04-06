using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public bool isVisible;

	// Use this for initialization
	void Start () {
		if (isVisible == false) 
		{
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
