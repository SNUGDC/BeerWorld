using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;


public class RoomManager : MonoBehaviour {
	
	public NetworkViewID Id;
    public string ipAddress = "147.46.76.106";
    public int port = 5000;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
        MasterServer.ipAddress = ipAddress;
        MasterServer.port = port;
        Id = Network.AllocateViewID();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateMasterServerInfo()
    {
        MasterServer.ipAddress = ipAddress;
		Network.natFacilitatorIP = ipAddress;
        MasterServer.port = port;
		Network.natFacilitatorPort = 5100;
        Id = Network.AllocateViewID();
    }

	void OnMasterServerEvent(MasterServerEvent msEvent){
        if (msEvent == MasterServerEvent.RegistrationSucceeded) {
            Debug.Log("Registered");
        }
	}

	void OnConnectedToServer(){
        Debug.Log("Connected");
		Network.SetLevelPrefix (1);
		Application.LoadLevel ("Battle"); // revise after complete Ready scene.
	}
	
	void OnServerInitialized(){
		Network.SetLevelPrefix (1);
		Application.LoadLevel ("Battle"); // revise after complete Ready scene.
	}

	void OnFailedToConnect(NetworkConnectionError error){
		Debug.Log ("Failed to Connect : " + error);
	}

	public void DisconnectFromServer(){
		Network.Disconnect (250);
		Application.LoadLevel ("Lobby");
		Destroy (this.gameObject);
	}
}
