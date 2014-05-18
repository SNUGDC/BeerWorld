using UnityEngine;
using System.Collections;

public class RoomGUIView : MonoBehaviour {
    public GUISkin guiSkin;
    
    // Use this for initialization
	void Start () {
        guiSkin.textField.fontSize = (int)(20 * Const.GUI_HEIGHT_UNIT);
        guiSkin.label.fontSize = (int)(20 * Const.GUI_HEIGHT_UNIT);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.skin = guiSkin;
        if(Network.peerType == NetworkPeerType.Server){
		    GUI.Label(new Rect(10*Const.GUI_WIDTH_UNIT,10*Const.GUI_HEIGHT_UNIT,200*Const.GUI_WIDTH_UNIT,40*Const.GUI_HEIGHT_UNIT),"Server");
			GUI.Label(new Rect(10*Const.GUI_WIDTH_UNIT,60*Const.GUI_HEIGHT_UNIT,200*Const.GUI_WIDTH_UNIT,40*Const.GUI_HEIGHT_UNIT),"# of Member : "+(Network.connections.Length+1));

			if(GUI.Button(new Rect(210*Const.GUI_WIDTH_UNIT,10*Const.GUI_HEIGHT_UNIT,200*Const.GUI_WIDTH_UNIT,40*Const.GUI_HEIGHT_UNIT), "Out")){
				Network.Disconnect(250);
                FindObjectOfType<ChatGUIView>().ClearTextToRead();
				Application.LoadLevel("Lobby");
				Destroy(this.gameObject);
			}
		}
		if(Network.peerType == NetworkPeerType.Client){
			GUI.Label(new Rect(10*Const.GUI_WIDTH_UNIT,10*Const.GUI_HEIGHT_UNIT,200*Const.GUI_WIDTH_UNIT,40*Const.GUI_HEIGHT_UNIT),"Client");
			GUI.Label(new Rect(10*Const.GUI_WIDTH_UNIT,60*Const.GUI_HEIGHT_UNIT,200*Const.GUI_WIDTH_UNIT,40*Const.GUI_HEIGHT_UNIT),"# of Member : "+(Network.connections.Length+1));

			if(GUI.Button(new Rect(210*Const.GUI_WIDTH_UNIT,10*Const.GUI_HEIGHT_UNIT,100*Const.GUI_WIDTH_UNIT,60*Const.GUI_HEIGHT_UNIT),"Out")){
				Network.Disconnect(250);
                FindObjectOfType<ChatGUIView>().ClearTextToRead();
				Application.LoadLevel("Lobby");
				Destroy(this.gameObject);
			}
		}
	}
}
