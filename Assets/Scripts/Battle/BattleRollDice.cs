using UnityEngine;
using System.Collections;

public class BattleRollDice : MonoBehaviour
{
    public BattleManager battleManager;

    void OnMouseDown()
    {
        battleManager.OnRollClicked();
    }
}
