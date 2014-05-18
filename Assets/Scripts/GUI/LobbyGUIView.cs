using UnityEngine;
using System.Collections;

public partial class LobbyGUIView : MonoBehaviour {

    public GUISkin guiSkin;
    private Rect createRoomRect;
    private Rect roomListRect;

    string roomName = "";
    string roomType = "";

	bool isCreating = false;
	bool isFinding = false;
	bool isReceived;

    // Use this for initialization
	void Start () {
        createRoomRect = new Rect(
            150 * Const.GUI_WIDTH_UNIT,
            60 * Const.GUI_HEIGHT_UNIT,
            300 * Const.GUI_WIDTH_UNIT,
            195 * Const.GUI_HEIGHT_UNIT
        );
        roomListRect = new Rect(
            150 * Const.GUI_WIDTH_UNIT,
            100 * Const.GUI_HEIGHT_UNIT,
            300 * Const.GUI_WIDTH_UNIT,
            195 * Const.GUI_HEIGHT_UNIT
        );
        guiSkin.textField.fontSize = (int)(20 * Const.GUI_HEIGHT_UNIT);
        guiSkin.label.fontSize = (int)(20 * Const.GUI_HEIGHT_UNIT);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.skin = guiSkin;
		if (Network.peerType == NetworkPeerType.Disconnected) {
            if (GUI.Button(new Rect(
                10 * Const.GUI_WIDTH_UNIT,
                10 * Const.GUI_HEIGHT_UNIT,
                150 * Const.GUI_WIDTH_UNIT,
                50 * Const.GUI_HEIGHT_UNIT
            ), "Create Room")) {
				isCreating = true;
			}
            if (GUI.Button(new Rect(
                10 * Const.GUI_WIDTH_UNIT,
                60 * Const.GUI_HEIGHT_UNIT,
                150 * Const.GUI_WIDTH_UNIT,
                50 * Const.GUI_HEIGHT_UNIT
            ), "Enter Room")) {
				isFinding = true;
				isReceived = false;
			}
            if (GUI.Button(new Rect(
                10 * Const.GUI_WIDTH_UNIT,
                110 * Const.GUI_HEIGHT_UNIT,
                150 * Const.GUI_WIDTH_UNIT,
                50 * Const.GUI_HEIGHT_UNIT
            ), "Log Out")) {
				Destroy(this.gameObject);
				Application.LoadLevel("Login");
			}
		}
		if(isCreating == true)
			createRoomRect = GUI.Window(1, createRoomRect, OnClickCreateRoom, "Create");
		if (isFinding == true)
			roomListRect = GUI.Window (2, roomListRect, OnClickRoomList, "RoomList");
    }

    private void OnClickCreateRoom(int id)
    {
        roomName = GUI.TextField(new Rect(
            15 * Const.GUI_WIDTH_UNIT,
            40 * Const.GUI_HEIGHT_UNIT,
            270 * Const.GUI_WIDTH_UNIT,
            40 * Const.GUI_HEIGHT_UNIT
        ), roomName);
        roomType = GUI.TextField(new Rect(
            15 * Const.GUI_WIDTH_UNIT,
            90 * Const.GUI_HEIGHT_UNIT,
            270 * Const.GUI_WIDTH_UNIT,
            40 * Const.GUI_HEIGHT_UNIT
        ), roomType);

        if (GUI.Button(new Rect(
            15 * Const.GUI_WIDTH_UNIT,
            140 * Const.GUI_HEIGHT_UNIT,
            135 * Const.GUI_WIDTH_UNIT,
            40 * Const.GUI_HEIGHT_UNIT
        ), "Create")) {
            CreateRoom(roomName, roomType);
        }
        if (GUI.Button(new Rect(
            150 * Const.GUI_WIDTH_UNIT,
            140 * Const.GUI_HEIGHT_UNIT,
            135 * Const.GUI_WIDTH_UNIT,
            40 * Const.GUI_HEIGHT_UNIT
        ), "Close")) {
            isCreating = false;
        }
        GUI.DragWindow();
    }

    private void OnClickRoomList(int id)
    {
        roomType = GUI.TextField(new Rect(15 * Const.GUI_WIDTH_UNIT, 40 * Const.GUI_HEIGHT_UNIT, 270 * Const.GUI_WIDTH_UNIT, 40 * Const.GUI_HEIGHT_UNIT), roomType);

        if (GUI.Button(new Rect(15 * Const.GUI_WIDTH_UNIT, 140 * Const.GUI_HEIGHT_UNIT, 135 * Const.GUI_WIDTH_UNIT, 40 * Const.GUI_HEIGHT_UNIT), "Search")) {
            SearchRoom();
        }
        if (GUI.Button(new Rect(150 * Const.GUI_WIDTH_UNIT, 140 * Const.GUI_HEIGHT_UNIT, 135 * Const.GUI_WIDTH_UNIT, 40 * Const.GUI_HEIGHT_UNIT), "Close")) {
            isFinding = false;
        }
        if (isReceived) {
            ShowHostList();
        }
        GUI.DragWindow();

    }

    private void ShowHostList()
    {
        HostData[] HostList = MasterServer.PollHostList();
        int i = 0;
        while (i < HostList.Length) {
            HostData element = HostList[i];
            if (GUI.Button(new Rect(10 * Const.GUI_WIDTH_UNIT, (60 + 25 * i) * Const.GUI_HEIGHT_UNIT, 180 * Const.GUI_WIDTH_UNIT, 25 * Const.GUI_HEIGHT_UNIT), HostList[i].gameType + ":" + HostList[i].gameName)) {
                JoinRoom(element);
            }
            i++;
        }
    }

}

public partial class LobbyGUIView : MonoBehaviour
{
    void CreateRoom(string roomName, string roomType)
    {
        Network.InitializeServer(10, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(roomType, roomName, "testing");
        isCreating = false;
    }
    void JoinRoom(HostData element)
    {
        Network.Connect(element);
        isFinding = false;
    }

    void SearchRoom()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList(roomType);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived) {
            Debug.Log("List Received");
            isReceived = true;
        }
    }
}