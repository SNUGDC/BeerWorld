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

    public GameObject[] playerAttackDices = new GameObject[3];
    public GameObject[] playerDefenseDices = new GameObject[3];
    public GameObject[] enemyAttackDices = new GameObject[3];
    public GameObject[] enemyDefenseDices = new GameObject[3]; 

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

    public void OnRollClicked()
    {
        BattleCalculator calculator = new BattleCalculator();
        if (attackOrDefense == AttackOrDefense.Attack)
        {
            playerCalcResult = calculator.GetAttackDiceResult(player);
            enemyCalcResult = calculator.GetDefenseDiceResult(enemy);
        }
        else
        {
            playerCalcResult = calculator.GetDefenseDiceResult(player);
            enemyCalcResult = calculator.GetAttackDiceResult(player);
        }

        Debug.Log("Player result is " + playerCalcResult.ToString());
        Debug.Log("Enemy result is " + enemyCalcResult.ToString());

        state = State.ShowRoll;
        AnimateDice();
    }

    void AnimateDice()
    {
        int playerDiceNum = playerCalcResult.diceResults.Count;
        int enemyDiceNum = enemyCalcResult.diceResults.Count;

        if (attackOrDefense == AttackOrDefense.Attack)
        {
            for (int i = 0; i < playerDiceNum; i++)
            {
                int diceResult = playerCalcResult.diceResults[i];
                playerAttackDices[i].SendMessage("rollByNumber", diceResult);    
            }
                    
            for (int i = 0; i < enemyDiceNum; i++)
            {
                int diceResult = enemyCalcResult.diceResults[i];
                enemyDefenseDices[i].SendMessage("rollByNumber", diceResult);    
            }
        }   
        else
        {
            for (int i = 0; i < playerDiceNum; i++)
            {
                int diceResult = playerCalcResult.diceResults[i];
                playerDefenseDices[i].SendMessage("rollByNumber", diceResult);    
            }

            for (int i = 0; i < enemyDiceNum; i++)
            {
                int diceResult = enemyCalcResult.diceResults[i];
                enemyAttackDices[i].SendMessage("rollByNumber", diceResult);    
            }

        }             

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
