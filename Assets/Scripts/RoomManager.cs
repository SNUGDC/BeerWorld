using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;


public class RoomManager : MonoBehaviour {
	
	NetworkViewID Id;
	
	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
        TextAsset config = Resources.Load<TextAsset>("Config/masterserver");
        if (config != null) {
            XmlTextReader reader = new XmlTextReader(new StringReader(config.text));
            reader.ReadToFollowing("ipAddress");
            MasterServer.ipAddress = reader.ReadElementContentAsString();
            reader.ReadToFollowing("port");
            MasterServer.port = reader.ReadElementContentAsInt();
        }
        Debug.Log(MasterServer.port);
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
