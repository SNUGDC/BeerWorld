using UnityEngine;
using System.Collections;

public partial class NetworkManager : MonoBehaviour
{
	public static void RequestPlayerData()
	{
		networkInstance.networkView.RPC("ReceiveRequestPlayerData", RPCMode.All);
	}

	[RPC]
	private void ReceiveRequestPlayerData()
	{
    SendPlayerData();
	}

  public static void SendPlayerData()
  {
    networkInstance.networkView.RPC("ReceiveSendPlayerData", RPCMode.All, NetworkManager.networkInstance.GetNetworkID());
  }

  [RPC]
  private void ReceiveSendPlayerData(NetworkViewID playerId)
  {
		BossBattleScene.Instance.AddUser(playerId);
  }
}
