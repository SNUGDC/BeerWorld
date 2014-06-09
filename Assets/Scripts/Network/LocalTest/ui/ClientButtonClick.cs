using UnityEngine;
using System.Collections;

public class ClientButtonClick : MonoBehaviour
{
    void OnMouseDown()
    {
        LocalTestNetwork.StartClient();
    }
}
