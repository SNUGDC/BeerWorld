using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadyManager : MonoBehaviour {
	List<NetworkViewID> totalPlayerList;
	bool isHost = false;
	NetworkViewID netID;


	// Use this for initialization
	void Start () {
		netID = Network.AllocateViewID ();
		if(Network.isServer)
			MakeHost();
		else
			networkView.RPC("ModifyList", RPCMode.Server, netID, true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeHost(){
		isHost = true;
	}

	[RPC]
	public void ModifyList(NetworkViewID playerID, bool changed){
		if(changed){
			totalPlayerList.Add (playerID);
			networkView.RPC("RefreshList", RPCMode.Others, totalPlayerList);
		}
	}

	[RPC]
	public void RefreshList(List<NetworkViewID> list){
		totalPlayerList = list;
	}
}
