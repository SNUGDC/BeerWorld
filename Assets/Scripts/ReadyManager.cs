using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadyManager : MonoBehaviour {
	Dictionary<NetworkViewID, MyCharacter> players = new Dictionary<NetworkViewID, MyCharacter>();
	List<NetworkViewID> totalPlayerList = new List<NetworkViewID>();
	List<GameObject> player = new List<GameObject>();
	bool isHost = false;
	NetworkViewID netID;
	MyCharacter myChar;


	// Use this for initialization
	void Start () {
		myChar = FindObjectOfType<MyCharacter> ();
		netID = Network.AllocateViewID ();
		if(Network.isServer)
			MakeHost();
		else
			networkView.RPC("ModifyList", RPCMode.Server, netID, myChar, true);
		player.Add (GameObject.Find ("playerTop"));
		player.Add (GameObject.Find ("playerMiddle"));
		player.Add (GameObject.Find ("playerBottom"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeHost(){
		isHost = true;
		totalPlayerList.Add (netID);
		players.Add (netID, myChar);
	}

	[RPC]
	public void ModifyList(NetworkViewID playerID, MyCharacter mChar, bool changed){
		if(changed){
			totalPlayerList.Add (playerID);
			players.Add(playerID,mChar);
			networkView.RPC("RefreshList", RPCMode.Others, totalPlayerList);
		}
	}

	[RPC]
	public void RefreshList(List<NetworkViewID> list){
		totalPlayerList = list;
		foreach(NetworkViewID id in list){
			if(id == this.netID)
				return;
		}
		this.myChar.myID = list.Count;
	}

	void ChangeList(){
		for(int i = 0 ; i<3; i++){
			if(i<totalPlayerList.Count){
				MyCharacter _player = players[totalPlayerList[i]];
				player[i].GetComponent<playersDisplay>().ChangeStatus(_player.currCharacter, i==myChar.myID, _player.ready);
			}
			else{
				player[i].GetComponent<playersDisplay>().ChangeStatus("empty", i==myChar.myID, false);
			}
		}
	}

	public void ApplyChange(){
		ChangeList ();
	}
}
