using UnityEngine;
using System.Collections;

public class ServerButtonClick : MonoBehaviour
{
    void OnMouseDown()
    {
        LocalTestNetwork.StartServer();
    }
}
