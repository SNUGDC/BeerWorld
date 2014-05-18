using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour {
	
	NetworkViewID Id;
	
	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		MasterServer.ipAddress = "147.46.241.250";
		MasterServer.port = 5000;
        Network.minimumAllocatableViewIDs = 3000;
		Id = Network.AllocateViewID ();
        Debug.Log(Id);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMasterServerEvent(MasterServerEvent msEvent){
        if (msEvent == MasterServerEvent.RegistrationSucceeded) {
            Debug.Log("Registered");
        }
	}

	void OnConnectedToServer(){
		Network.SetLevelPrefix (1);
		Application.LoadLevel ("Game");
	}
	
	void OnServerInitialized(){
		Network.SetLevelPrefix (1);
		Application.LoadLevel ("Game");
	}
    [RPC]
    private void SendMsg(string id, string msg)
    {
        FindObjectOfType<ChatGUIView>().Chat(id, msg);
    }
}
