using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public static NetworkManager Get()
	{
		return networkInstance;
	}

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
		Id = Network.AllocateViewID ();
	}

	// Use this for initialization
	void Start () {
		username = PlayerPrefs.GetString ("id");
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
		Debug.Log("Dice of another player : " + diceResult);
		GameManager.GetMyCharacterManager().SetMovement(diceResult);
	}

	public static void SetTurnOrder(NetworkViewID ownerId, List<NetworkViewID> otherPlayers)
	{
		if (otherPlayers.Count == 0)
		{
			networkInstance.networkView.RPC("ReceiveSetTurnOrder_1", RPCMode.AllBuffered, ownerId);
		}
		else if (otherPlayers.Count == 1)
		{
			networkInstance.networkView.RPC("ReceiveSetTurnOrder_2", RPCMode.AllBuffered, ownerId, otherPlayers[0]);
		}
		else if (otherPlayers.Count == 2)
		{
			networkInstance.networkView.RPC("ReceiveSetTurnOrder_3", RPCMode.AllBuffered, ownerId, otherPlayers[0], otherPlayers[1]);
		}
		else
		{
			Debug.LogError("Invalid Player Count : " + (1 + otherPlayers.Count));
		}
	}

	[RPC]
	private void ReceiveSetTurnOrder_1(NetworkViewID firstPlayer)
	{
		GameManager.gameManagerInstance.SetTurnOrder(new List<NetworkViewID>{
			firstPlayer
		});
	}

	[RPC]
	private void ReceiveSetTurnOrder_2(NetworkViewID firstPlayer, NetworkViewID secondPlayer)
	{
		GameManager.gameManagerInstance.SetTurnOrder(new List<NetworkViewID>{
			firstPlayer, secondPlayer
		});
	}

	[RPC]
	private void ReceiveSetTurnOrder_3(NetworkViewID firstPlayer, NetworkViewID secondPlayer, NetworkViewID thirdPlayer)
	{
		GameManager.gameManagerInstance.SetTurnOrder(new List<NetworkViewID>{
			firstPlayer, secondPlayer, thirdPlayer
		});
	}

	public static void SendGameStartMessage ()
	{
		networkInstance.networkView.RPC("ReceiveGameStartMessage", RPCMode.Others, networkInstance.Id);
		Debug.Log("Game Start! @Server");
	}

	[RPC]
	private void ReceiveGameStartMessage(NetworkViewID id)
	{
		GameManager.gameManagerInstance.CreateCharacters();
		Debug.Log("Game Start! @Cilent");
	}

    //temp
    static Character.CharClass RandomSelectClass()
    {
        int temp = Random.Range(1,3);
        switch (temp)
        {
            case 1:
                return Character.CharClass.Warrior;
            case 2:
                return Character.CharClass.Tanker;
            case 3:
                return Character.CharClass.Attacker;
            default:
                return Character.CharClass.Novice;
        }
    }

	public static void SendUsersNetworkViewID ()
	{
        //Character.CharClass charClass = RandomSelectClass();
		Character.CharClass charClass = (Character.CharClass) Character.CharClass.Parse(typeof(Character.CharClass),GameObject.Find("RoomManager").GetComponent<RoomManager>().getCharacter());
        if (charClass == Character.CharClass.Warrior)
        {
            networkInstance.networkView.RPC("ReceiveWarriorUsersNetworkViewID", RPCMode.AllBuffered, networkInstance.Id);
        }
        else if (charClass == Character.CharClass.Tanker)
        {
            networkInstance.networkView.RPC("ReceiveTankerUsersNetworkViewID", RPCMode.AllBuffered, networkInstance.Id);
        }
        else if (charClass == Character.CharClass.Attacker)
        {
            networkInstance.networkView.RPC("ReceiveAttackerUsersNetworkViewID", RPCMode.AllBuffered, networkInstance.Id);
        }
        else
        {
            networkInstance.networkView.RPC("ReceiveNoviceUsersNetworkViewID", RPCMode.AllBuffered, networkInstance.Id);
        }
    }	

	[RPC]
	private void ReceiveNoviceUsersNetworkViewID(NetworkViewID id)
	{
        Character.CharClass charClass = Character.CharClass.Novice;
		GameManager.gameManagerInstance.AddUser(id, charClass);
	}

    [RPC]
    private void ReceiveWarriorUsersNetworkViewID(NetworkViewID id)
    {
        Character.CharClass charClass = Character.CharClass.Warrior;
        GameManager.gameManagerInstance.AddUser(id, charClass);
    }

    [RPC]
    private void ReceiveTankerUsersNetworkViewID(NetworkViewID id)
    {
        Character.CharClass charClass = Character.CharClass.Tanker;
        GameManager.gameManagerInstance.AddUser(id, charClass);
    }

    [RPC]
    private void ReceiveAttackerUsersNetworkViewID(NetworkViewID id)
    {
        Character.CharClass charClass = Character.CharClass.Attacker;
        GameManager.gameManagerInstance.AddUser(id, charClass);
    }

    [RPC]
    private void ReceiveThiefUsersNetworkViewID(NetworkViewID id)
    {
        Character.CharClass charClass = Character.CharClass.Thief;
        GameManager.gameManagerInstance.AddUser(id, charClass);
    }

	public static void SendMoveTile(int coordX, int coordY)
	{
		networkInstance.networkView.RPC("ReceiveMoveTile", RPCMode.Others,
				networkInstance.Id, coordX, coordY);
	}

	[RPC]
	private void ReceiveMoveTile(NetworkViewID id, int coordX, int coordY)
	{
		Run.Coroutine(GameManager.GetCharacterManager(id).MoveByNetwork(coordX, coordY));
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
		BattleUIManager.Get().SetPlayerTurn(nextPlayerId);
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

	public static void StartEnemyTurn()
	{
		networkInstance.networkView.RPC("ReceiveStartEnemyTurn", RPCMode.All);
	}

	[RPC]
	private void ReceiveStartEnemyTurn()
	{
		BattleUIManager.Get().SetEnemyTrn();
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

	public static void SyncTurnCount(int turnCount)
	{
		networkInstance.networkView.RPC("ReceiveSyncTurnCount", RPCMode.All, turnCount);
	}

	[RPC]
	private void ReceiveSyncTurnCount(int turnCount)
	{
		BattleUIManager.Get().SetTurnCount(turnCount);
	}

	public static void ShowEnemyTurn()
	{
		networkInstance.networkView.RPC("ReceiveShowEnemyTurn", RPCMode.All);
	}

	[RPC]
	private void ReceiveShowEnemyTurn()
	{
		BattleUIManager.Get().ShowEnemyTurn();
	}
}
