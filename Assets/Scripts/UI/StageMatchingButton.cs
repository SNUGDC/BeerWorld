using UnityEngine;
using System.Collections;

public partial class StageMatchingButton : MonoBehaviour {
	RoomManager rmManager;
	public string mapName = "France"; 

	// Use this for initialization
	void Start () {
		rmManager = FindObjectOfType<RoomManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Debug.Log ("Clicked");
		rmManager.UpdateMasterServerInfo();
		SearchRoom ("France");
	}

	void OnQueue(){
		HostData[] roomList = MasterServer.PollHostList();
		if(roomList.Length == 0){
			CreateRoom(SystemInfo.deviceUniqueIdentifier, mapName);
			Debug.Log("No Room in Server. So Create the Room.");
			return;
		}
		else{
			foreach (HostData room in roomList){
				if(room.connectedPlayers<3){
					JoinRoom(room);
					Debug.Log("Find Room. So Enter the Room.");
					return;
				}
			}
			CreateRoom(SystemInfo.deviceUniqueIdentifier, mapName);
			Debug.Log("There Are No Room Available. So Create the Room.");
			return;
		}
	}
}

public partial class StageMatchingButton : MonoBehaviour
{
	void CreateRoom(string roomName, string roomType)
	{
		Network.InitializeServer(3, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(roomType, roomName, "testing");
	}
	
	void JoinRoom(HostData element)
	{
		NetworkConnectionError error = Network.Connect(element);
		Debug.Log("Join Room : " + error);
	}
	
	void SearchRoom(string roomType)
	{
		Debug.Log ("Search Room. " + "MasterServerIP : " + MasterServer.ipAddress + "Port : " + MasterServer.port);
		MasterServer.ClearHostList();
		MasterServer.RequestHostList(roomType);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived) {
			Debug.Log("List Received");
			OnQueue();
		}
	}
}