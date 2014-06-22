using UnityEngine;
using System.Collections;

public class ChatGUIView : MonoBehaviour {

    private Rect chatWindowRect;
    private string username = "";
    private Vector2 ScrollPos = new Vector2(200, 10);
    private string textToread = "";
    public GUISkin guiSkin;

    public void ClearTextToRead()
    {
        textToread = "";
    }
    // Use this for initialization
	void Start () {
        Application.RegisterLogCallback(HandleLog);

        chatWindowRect = new Rect(
            0 * Const.GUI_WIDTH_UNIT,
            Screen.height - 300 * Const.GUI_HEIGHT_UNIT,
            500 * Const.GUI_WIDTH_UNIT, 300 * Const.GUI_HEIGHT_UNIT
        );
        username = PlayerPrefs.GetString("id");
        guiSkin.textField.fontSize = (int)(20 * Const.GUI_HEIGHT_UNIT);
        guiSkin.label.fontSize = (int)(20 * Const.GUI_HEIGHT_UNIT);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGUI()
    {
        GUI.skin = guiSkin;
        chatWindowRect = GUI.Window(0, chatWindowRect, OnChatGUI, "Chat");
    }

    private void OnChatGUI(int id)
    {
        ScrollPos = GUILayout.BeginScrollView(ScrollPos);
        GUILayout.Label(textToread);
        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Hi")) {
            FindObjectOfType<RoomManager>().networkView.RPC("SendMsg", RPCMode.All, username, "Hi");
        }
        if (GUILayout.Button("Bye")) {
            FindObjectOfType<RoomManager>().networkView.RPC("SendMsg", RPCMode.All, username, "Bye");
        }
        if (GUILayout.Button("lol")) {
            FindObjectOfType<RoomManager>().networkView.RPC("SendMsg", RPCMode.All, username, "lol");
        }
        if (GUILayout.Button("woowoo")) {
            FindObjectOfType<RoomManager>().networkView.RPC("SendMsg", RPCMode.All, username, "woowoo");
        }
        GUILayout.EndHorizontal();
    }

    public void Chat(string id, string msg)
    {
        textToread += id + ": " + msg + "\n";
        ScrollPos.y += 100;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        textToread += logString + "\n";
    }

}
