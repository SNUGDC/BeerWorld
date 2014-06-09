using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	public GameObject target;
	GameObject player;
	string username;
	NetworkViewID Id;

    private static NetworkManager networkInstance = null;

    void Awake ()
    {
        networkInstance = this;
    }

	// Use this for initialization
	void Start () {
		username = PlayerPrefs.GetString ("id");
		Id = Network.AllocateViewID ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A)){
			
			//networkView.RPC("StartGame", RPCMode.AllBuffered, Id, transform.position, Quaternion.identity);
			player = Network.Instantiate (target, transform.position, Quaternion.identity,0 ) as GameObject;
			
		}
		if(Input.GetKeyDown(KeyCode.D)){
			Network.Destroy(player);
		}
		if(Input.GetKey(KeyCode.W)){
			player.transform.position += new Vector3(0,1,0);
		}
		if(Input.GetKey(KeyCode.S)){
			player.transform.position += new Vector3(0,-1,0);
		}
	
	}
	
	[RPC]
	private void StartGame(NetworkViewID id, Vector3 pos, Quaternion rot){
		//implement if func when started
	}

    public static void SendRollDice(int diceResult)
    {
        networkInstance.networkView.RPC("ReceiveRollDice", RPCMode.Others, networkInstance.Id, diceResult);
    }

    [RPC]
    private void ReceiveRollDice(NetworkViewID id, int diceResult)
    {
        //targetFunction.variable = diceResult;
        Debug.Log("Dice of another player : " + diceResult);
        CharacterManager.characterManagerInstance.SetMovement(diceResult);
    }

    public static void SendGameStartMessage ()
    {
        networkInstance.networkView.RPC("ReceiveGameStartMessage", RPCMode.Others, networkInstance.Id);
        Debug.Log("Game Start! @Server");
    }

    [RPC]
    private void ReceiveGameStartMessage(NetworkViewID id)
    {
        Debug.Log("Game Start! @Cilent");
        SendUsersNetworkViewID();
    }

    public static void SendUsersNetworkViewID ()
    {
        networkInstance.networkView.RPC("ReceiveUsersNetworkViewID", RPCMode.Others, networkInstance.Id);
    }

    [RPC]
    private void ReceiveUsersNetworkViewID(NetworkViewID id)
    {
        Debug.Log("SendID : " + id);
    }

    public static void SendMoveTile(int coordX, int coordY)
    {
        networkInstance.networkView.RPC("ReceiveMoveTile", RPCMode.Others, coordX, coordY);
    }

    [RPC]
    private void ReceiveMoveTile(NetworkViewID id, int coordX, int coordY)
    {
        Debug.Log("Move tile to " + coordX + ", " + coordY);
    }
}
