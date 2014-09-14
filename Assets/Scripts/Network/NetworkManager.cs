using UnityEngine;
using System.Collections;

public partial class NetworkManager : MonoBehaviour {
	public GameObject target;
	GameObject player;
	string username;
	public NetworkViewID Id
	{
		get;
		private set;
	}

	public static NetworkManager networkInstance = null;

	public static bool isConnected()
	{
		return Network.connections.Length > 0;
	}

	public NetworkViewID GetNetworkID()
	{
		return Id;
	}

	void Awake ()
	{
		networkInstance = this;
	}

	// Use this for initialization
	void Start () {
		username = PlayerPrefs.GetString ("id");
		Id = Network.AllocateViewID ();
	}

	[RPC]
	private void StartGame(NetworkViewID id, Vector3 pos, Quaternion rot){
		//implement if func when started
	}

	//public static void Send

	public static void SendRollDice(int diceResult)
	{
		networkInstance.networkView.RPC("ReceiveRollDice", RPCMode.Others, networkInstance.Id, diceResult);
	}

	[RPC]
	private void ReceiveRollDice(NetworkViewID id, int diceResult)
	{
		Debug.Log("Dice of another player : " + diceResult);
		GameManager.GetMyCharacterManager().SetMovement(diceResult);
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
		GameManager.gameManagerInstance.AddUser(id);
		//
	}

	public static void SendMoveTile(int coordX, int coordY)
	{
		networkInstance.networkView.RPC("ReceiveMoveTile", RPCMode.Others,
				networkInstance.Id, coordX, coordY);
	}

	[RPC]
	private void ReceiveMoveTile(NetworkViewID id, int coordX, int coordY)
	{
		GameManager.GetCharacterManager(id).Move(coordX, coordY);
		//GameManager.GetMyCharacterManager().MoveCharacter(coordX, coordY);
		Debug.Log("Move tile to " + coordX + ", " + coordY);
	}

	public static void SendTurnEndMessage ()
	{
		networkInstance.networkView.RPC("ReceiveTurnEndMessage", RPCMode.All, networkInstance.Id);
	}

	[RPC]
	private void ReceiveTurnEndMessage(NetworkViewID id)
	{
		if (Network.isClient == false)
		{
			GameManager.gameManagerInstance.PassTurnToNextPlayer();
		}
	}

	public static void SendTurnStartMessage (NetworkViewID nextPlayerId)
	{
		networkInstance.networkView.RPC("ReceiveTurnStartMessage", RPCMode.All, nextPlayerId);
	}

	[RPC]
	private void ReceiveTurnStartMessage(NetworkViewID nextPlayerId)
	{
		if (NetworkManager.networkInstance.Id == nextPlayerId)
		{
			GameManager.GetMyCharacterManager().ChangeMoveStateToIdle();
			Debug.Log("My turn");
		}
		else
		{
			Debug.Log("Not My turn");
		}
	}

	public static void MoveToBossBatte()
	{
		networkInstance.networkView.RPC("ReceiveMoveToBossBattle", RPCMode.All);
	}

	[RPC]
	private void ReceiveMoveToBossBattle()
	{
		Application.LoadLevel("BossBattle");
	}
}
