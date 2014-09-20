using UnityEngine;
using System.Collections;

public class playersDisplay : MonoBehaviour {
	SpriteRenderer spRender;
	GameObject highLight;
	GameObject status;

	// Use this for initialization
	void Start () {
		spRender = GetComponent<SpriteRenderer> ();
		highLight = GameObject.Find (this.gameObject.name + "/highLight");
		status = GameObject.Find (this.gameObject.name + "/status");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeStatus(string charType, bool isMe, bool onWaiting){
		spRender.sprite = Resources.Load ("Images/mainUI/components/lobby_player_" + charType, typeof(Sprite)) as Sprite;
		if(isMe)
			highLight.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Images/mainUI/components/lobby_player_mine", typeof(Sprite)) as Sprite;
		else
			highLight.GetComponent<SpriteRenderer> ().sprite = Resources.Load (null, typeof(Sprite)) as Sprite;
		if(onWaiting)
			status.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Images/mainUI/components/lobby_player_ready", typeof(Sprite)) as Sprite;
		else if(charType!="empty")
			status.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Images/mainUI/components/lobby_player_wait", typeof(Sprite)) as Sprite;
		else
			status.GetComponent<SpriteRenderer> ().sprite = Resources.Load (null, typeof(Sprite)) as Sprite;
	}
}
