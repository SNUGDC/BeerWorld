using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileAudioClip : MonoBehaviour {
	public AudioClip[] Clips;
	public AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
		audioSources = new AudioSource[Clips.Length];
		
		int i = 0;
		
		while (i < Clips.Length)
			
		{
			
			GameObject child = new GameObject("Sound");
			
			child.transform.parent = gameObject.transform;
			
			audioSources[i] = child.AddComponent("AudioSource") as AudioSource;
			
			audioSources[i].clip = Clips[i];
			
			i++;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
