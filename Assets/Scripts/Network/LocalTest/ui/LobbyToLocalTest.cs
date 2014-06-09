using UnityEngine;
using System.Collections;

public class LobbyToLocalTest : MonoBehaviour
{
    void OnMouseDown()
    {
        Application.LoadLevel("LocalTest");
    }
}
