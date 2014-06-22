using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    enum State
    {
        Inactive,
        Start,
        WaitingRoll,
        ShowRoll,
        ShowDamage,
        BattleEnd
    }

    enum AttackOrDefense
    {
        Attack,
        Defense
    }

    State state = State.Inactive;
    AttackOrDefense attackOrDefense = AttackOrDefense.Attack;
    BattlePlayer player;
    BattlePlayer enemy;
    CalculationResult playerCalcResult = null;
    CalculationResult enemyCalcResult = null;
    public Camera battleCamera;

    public void ShowBattle()
    {
        battleCamera.enabled = true;
        state = State.Start;
    }

    public void EndBattle()
    {
        battleCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (state == State.WaitingRoll)
            {
                OnRollClicked();
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            state = State.Start;
        }

        if (state == State.Start)
        {
            player = BattleUtil.GetDummyPlayer();
            enemy = BattleUtil.GetDummyEnemy();
            state = State.WaitingRoll;
        }
        else if (state == State.BattleEnd)
        {
            Debug.Log("BattleENd.");
            state = State.Inactive;
            EndBattle();
        }
    }

    void OnRollClicked()
    {
        BattleCalculator calculator = new BattleCalculator();
        if (attackOrDefense == AttackOrDefense.Attack)
        {
            playerCalcResult = calculator.GetAttackDiceResult(player);
            enemyCalcResult = calculator.GetDefenceDiceResult(enemy);
        }
        else
        {
            playerCalcResult = calculator.GetDefenceDiceResult(player);
            enemyCalcResult = calculator.GetAttackDiceResult(player);
        }

        Debug.Log("Player result is " + playerCalcResult.ToString());
        Debug.Log("Enemy result is " + enemyCalcResult.ToString());

        state = State.ShowRoll;
        AnimateDice();
    }

    void AnimateDice()
    {
        //show animation with calculation result.
        state = State.ShowDamage;
        AnimateDamage();
    }

    void AnimateDamage()
    {
        //show animation with calculation result.
        //apply damage.
        state = State.WaitingRoll;
        enemy.ApplyDamage(1);

        if (enemy.IsDie())
        {
            state = State.BattleEnd;
        }
    }
}
