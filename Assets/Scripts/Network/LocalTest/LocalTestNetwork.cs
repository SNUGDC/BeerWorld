using UnityEngine;
using System.Collections;

public class LocalTestNetwork : MonoBehaviour
{
    public static readonly int ServerPortNum = 25252;
    public static readonly int ClientPortNum = 25253;

    public static void StartServer()
    {
        Debug.Log("Start Server.");
        Network.InitializeServer(10, ServerPortNum, false);
    }

    public static void StartClient()
    {
        Debug.Log("Start Client.");
        Network.Connect("localhost", ServerPortNum);
    }
}
