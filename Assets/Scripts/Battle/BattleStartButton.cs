using UnityEngine;
using System.Collections;

public class BattleStartButton : MonoBehaviour
{
    public BattleManager battleManager;

    void OnMouseDown()
    {
        battleManager.ShowBattle();
    }
}
