using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour {
	
	private Vector2 ScrollPos=new Vector2(200,10);
	private Rect ChatWindow;
	private Rect CreateRoom;
	private Rect RoomList;
	public GUISkin guiskin;
	float height_unit = Screen.height / 720f * 1.4f;
	float width_unit =  Screen.width / 1280f * 1.4f;
	string textToread="";
	string username;
	string RoomName = "";
	string RoomType = "";
	HostData[] HostList;
	bool received;
	bool iscreating = false;
	bool isfinding = false;
	NetworkViewID Id;
	
	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		MasterServer.ipAddress = "147.46.241.250";
		MasterServer.port = 5000;
		username = PlayerPrefs.GetString ("id");
		ChatWindow = new Rect(0*width_unit,Screen.height-300*height_unit,500*width_unit,300*height_unit);
		CreateRoom = new Rect (150*width_unit, 60*height_unit, 300*width_unit, 195*height_unit);
		RoomList = new Rect (150*width_unit, 100*height_unit, 300*width_unit, 195*height_unit);
		guiskin.textField.fontSize = (int)(20 * height_unit);
		guiskin.label.fontSize = (int)(20 * height_unit);
		Id = Network.AllocateViewID ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.skin = guiskin;
		if (Network.peerType == NetworkPeerType.Disconnected) {
			if(GUI.Button(new Rect(10*width_unit,10*height_unit,150*width_unit,50*height_unit), "Create Room")){
				iscreating = true;
				Debug.Log("Create");
			}
			if(GUI.Button(new Rect(10*width_unit,60*height_unit,150*width_unit,50*height_unit), "Enter Room")){
				isfinding = true;
				received = false;
			}
			if(GUI.Button(new Rect(10*width_unit,110*height_unit,150*width_unit,50*height_unit), "Log Out")){
				Destroy(this.gameObject);
				Application.LoadLevel("Login");
			}
		}
		else {
			if(Network.peerType == NetworkPeerType.Server){
				GUI.Label(new Rect(10*width_unit,10*height_unit,200*width_unit,40*height_unit),"Server");
				GUI.Label(new Rect(10*width_unit,60*height_unit,200*width_unit,40*height_unit),"# of Member : "+(Network.connections.Length+1));
				
				if(GUI.Button(new Rect(210*width_unit,10*height_unit,200*width_unit,40*height_unit), "Out")){
					Network.Disconnect(250);
					textToread ="";
					Application.LoadLevel("Lobby");
					Destroy(this.gameObject);
				}
			}
			if(Network.peerType == NetworkPeerType.Client){
				GUI.Label(new Rect(10*width_unit,10*height_unit,200*width_unit,40*height_unit),"Client");
				GUI.Label(new Rect(10*width_unit,60*height_unit,200*width_unit,40*height_unit),"# of Member : "+(Network.connections.Length+1));
				
				if(GUI.Button(new Rect(210*width_unit,10*height_unit,100*width_unit,60*height_unit),"Out")){
					Network.Disconnect(250);
					textToread = "";
					Application.LoadLevel("Lobby");
					Destroy(this.gameObject);
				}
			}
			ChatWindow = GUI.Window(0, ChatWindow, Window_Func, "Chat");
		}
		if(iscreating == true)
			CreateRoom = GUI.Window(1, CreateRoom, CreateRoom_func, "Create");
		if (isfinding == true)
			RoomList = GUI.Window (2, RoomList, RoomList_func, "RoomList");

	}
	private void Window_Func(int id){
		ScrollPos = GUILayout.BeginScrollView(ScrollPos);
		GUILayout.Label(textToread);
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal ();

		if(GUILayout.Button("Hi")){
			networkView.RPC("SendMsg", RPCMode.All, username, "Hi");
		}
		if(GUILayout.Button("Bye")){
			networkView.RPC("SendMsg", RPCMode.All, username, "Bye");
		}
		if(GUILayout.Button("lol")){
			networkView.RPC("SendMsg", RPCMode.All, username, "lol");
		}
		if(GUILayout.Button("woowoo")){
			networkView.RPC("SendMsg", RPCMode.All, username,"woowoo");
		}
		GUILayout.EndHorizontal ();
	}
	
	private void CreateRoom_func(int id){
		RoomName = GUI.TextField(new Rect(15*width_unit,40*height_unit, 270*width_unit,40*height_unit), RoomName);
		RoomType = GUI.TextField(new Rect(15*width_unit,90*height_unit,270*width_unit,40*height_unit), RoomType);

		if(GUI.Button(new Rect(15*width_unit,140*height_unit,135*width_unit,40*height_unit), "Create")){
			Network.InitializeServer(10,25000,!Network.HavePublicAddress());
			MasterServer.RegisterHost(RoomType,RoomName,"testing");
			iscreating = false;
		}
		if(GUI.Button(new Rect(150*width_unit,140*height_unit,135*width_unit,40*height_unit), "Close"))
			iscreating = false;
		GUI.DragWindow ();
	}
	
	private void RoomList_func(int id){
		RoomType = GUI.TextField(new Rect(15*width_unit,40*height_unit,270*width_unit,40*height_unit), RoomType);

		if(GUI.Button(new Rect(15*width_unit,140*height_unit,135*width_unit,40*height_unit), "Search")){
			MasterServer.ClearHostList();
			MasterServer.RequestHostList(RoomType);
		}
		if(GUI.Button(new Rect(150*width_unit,140*height_unit,135*width_unit,40*height_unit),"Close"))
			isfinding = false;
		if(received){
			HostList = MasterServer.PollHostList();
			int i = 0;
			while(i<HostList.Length){
				HostData element = HostList[i];
				if(GUI.Button(new Rect(10*width_unit,(60+25*i)*height_unit, 180*width_unit,25*height_unit),HostList[i].gameType+":"+HostList[i].gameName)){
					Network.Connect(element);
					isfinding = false;
				}
				i++;
			}
		}
		GUI.DragWindow ();
		
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent){
		if (msEvent == MasterServerEvent.RegistrationSucceeded)
			Debug.Log ("Registered");
		if (msEvent == MasterServerEvent.HostListReceived) {
			Debug.Log ("List Received");
			received = true;
		}
	}

	[RPC]
	private void SendMsg(string id, string msg){
		textToread += id + ": " + msg + "\n";
		ScrollPos.y += 100;
	}
	
	void OnConnectedToServer(){
		Network.SetLevelPrefix (1);
		Application.LoadLevel ("TestField");
	}
	
	void OnServerInitialized(){
		Network.SetLevelPrefix (1);
		Application.LoadLevel ("TestField");
	}
}
