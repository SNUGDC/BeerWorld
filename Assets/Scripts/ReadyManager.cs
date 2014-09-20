using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadyManager : MonoBehaviour {
	//Dictionary<NetworkViewID, MyCharacter> players = new Dictionary<NetworkViewID, MyCharacter>();
	List<NetworkViewID> totalPlayerList = new List<NetworkViewID>();
	List<GameObject> player = new List<GameObject>();
	bool[] isFullReady = new bool[3];
	bool isHost = false;
	NetworkViewID netID;
	MyCharacter myChar;


	// Use this for initialization
	void Start () {
		Network.minimumAllocatableViewIDs = 500;
		myChar = FindObjectOfType<MyCharacter> ();
		netID = Network.AllocateViewID ();
		if(Network.isServer){
			MakeHost();
		}
		else{
			networkView.RPC("AddList", RPCMode.Server, netID);
		}
		player.Add (GameObject.Find ("playerTop"));
		player.Add (GameObject.Find ("playerMiddle"));
		player.Add (GameObject.Find ("playerBottom"));
		ApplyChange ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeHost(){
		isHost = true;
		totalPlayerList.Add (netID);
	}

	[RPC]
	public void AddList(NetworkViewID playerID){
		int num = totalPlayerList.Count;
		totalPlayerList.Add (playerID);
		foreach(NetworkViewID _player in totalPlayerList){
			networkView.RPC("RefreshList", RPCMode.Others, _player, num);
		}
		networkView.RPC ("ApplyChange", RPCMode.AllBuffered);
	}

	[RPC]
	public void RefreshList(NetworkViewID ID, int num){

		foreach(NetworkViewID id in totalPlayerList){
			if(id == this.netID){
				totalPlayerList.Add(ID);
				return;
			}
		}
		totalPlayerList.Add (ID);
		this.myChar.myID = num;
	}

	[RPC]
	void ChangeList(string charName, int ID, int isReady){
		player[ID].GetComponent<playersDisplay>().ChangeStatus(charName, ID==myChar.myID, isReady==1);
		/*for(int i = 0 ; i<3; i++){
			if(i<totalPlayerList.Count){
				MyCharacter _player = players[totalPlayerList[i]];
				player[i].GetComponent<playersDisplay>().ChangeStatus(_player.currCharacter, i==myChar.myID, _player.ready);
			}
			else{
				player[i].GetComponent<playersDisplay>().ChangeStatus("empty", i==myChar.myID, false);
			}
		}*/
	}

	[RPC]
	public void ApplyChange(){
		networkView.RPC ("ChangeList", RPCMode.All, myChar.currCharacter, myChar.myID, (myChar.ready)?1:0);
	}

	[RPC]
	public void AmIReady(int ID, int ready){
		Debug.Log ("Someone pushed Ready Button");
		isFullReady[ID] = (ready==1);
		if(CheckReady())
			networkView.RPC("GameStart", RPCMode.All);
	}

	bool CheckReady(){
		if(totalPlayerList.Count != 3){
			Debug.Log("# of total player is not 3");
			return false;
		}
		foreach(bool ready in isFullReady){
			if(!ready){
				Debug.Log("Someone couldn't ready");
				return false;
			}
		}
		return true;
	}

	[RPC]
	void GameStart(){
		Application.LoadLevel ("Battle");
	}
}
