﻿using UnityEngine;
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
    public GameObject[] playerDefenceDices = new GameObject[3];
    public GameObject[] enemyAttackDices = new GameObject[3];
    public GameObject[] enemyDefenceDices = new GameObject[3]; 

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
        int playerDiceNum = playerCalcResult.diceResults.Count;
/*        int firstDiceResult = playerCalcResult.diceResults[0];
        int secondDiceResult = playerCalcResult.diceResults[1];
        int thirdDiceResult = playerCalcResult.diceResults[2];

        playerAttackDices[0].SendMessage("rollByNumber", firstDiceResult);
        playerAttackDices[1].SendMessage("rollByNumber", secondDiceResult);
        playerAttackDices[2].SendMessage("rollByNumber", thirdDiceResult);

        playerDefenceDices[0].SendMessage("rollByNumber", firstDiceResult);
        playerDefenceDices[1].SendMessage("rollByNumber", secondDiceResult);
        playerDefenceDices[2].SendMessage("rollByNumber", thirdDiceResult);
*/
        for (int i = 0; i < playerDiceNum; i++)
        {
            int diceResult = playerCalcResult.diceResults[i];
            playerAttackDices[0].SendMessage("rollByNumber", diceResult);    
        }
        
        for (int i = 0; i < playerDiceNum; i++)
        {
            int diceResult = playerCalcResult.diceResults[i];
            playerDefenceDices[0].SendMessage("rollByNumber", diceResult);    
        }

        int enemyDiceNum = enemyCalcResult.diceResults.Count;
/*        firstDiceResult = enemyCalcResult.diceResults[0];
        secondDiceResult = enemyCalcResult.diceResults[1];
        thirdDiceResult = enemyCalcResult.diceResults[2];
        
        enemyAttackDices[0].SendMessage("rollByNumber", firstDiceResult);
        enemyAttackDices[1].SendMessage("rollByNumber", secondDiceResult);
        enemyAttackDices[2].SendMessage("rollByNumber", thirdDiceResult);
        
        enemyDefenceDices[0].SendMessage("rollByNumber", firstDiceResult);
        enemyDefenceDices[1].SendMessage("rollByNumber", secondDiceResult);
        enemyDefenceDices[2].SendMessage("rollByNumber", thirdDiceResult);
*/
        for (int i = 0; i < enemyDiceNum; i++)
        {
            int diceResult = enemyCalcResult.diceResults[i];
            enemyAttackDices[0].SendMessage("rollByNumber", diceResult);    
        }
        
        for (int i = 0; i < enemyDiceNum; i++)
        {
            int diceResult = enemyCalcResult.diceResults[i];
            enemyDefenceDices[0].SendMessage("rollByNumber", diceResult);    
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
