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

    void changeAttackOrDefense()
    {
        if (attackOrDefense == AttackOrDefense.Attack)
        {
            attackOrDefense = AttackOrDefense.Defense;
        }
        else 
        {
            attackOrDefense = AttackOrDefense.Attack;
        }
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
        AnimateDamage(playerDiceNum, enemyDiceNum);
    }

    BattlePlayer CompareDamageAndSelectTarget(int playerDiceNum, int enemyDiceNum)
    {
        if (playerDiceNum > enemyDiceNum)
        {
            return enemy;
        }
        else if (playerDiceNum < enemyDiceNum) 
        {
            return player;
        }
        else
        {
            Debug.Log("Dice is same.");
            return null;
        }
    }

    int CalculateDamage(int playerDiceNum, int enemyDiceNum)
    {
        return System.Math.Abs(playerDiceNum - enemyDiceNum);
    }

    void AnimateDamage(int playerDiceNum, int enemyDiceNum)
    {
        BattlePlayer target = null;
        int damage = 0;
        
//        CompareDamageAndSelectTarget(playerDiceNum, enemyDiceNum);
        damage = CalculateDamage(playerDiceNum, enemyDiceNum);
        target = CompareDamageAndSelectTarget(playerDiceNum, enemyDiceNum);

        Debug.Log("PlayerDice : " + playerDiceNum + ", EnemyDice : " + enemyDiceNum);
        //show animation with calculation result.
        //apply damage.
        state = State.WaitingRoll;
        target.ApplyDamage(damage);
        if (target == enemy)
        {
            Debug.Log("Enemy is Damaged " + damage);
        }
        else if (target == player)
        {
            Debug.Log("Player is Damaged " + damage);
        }
        
        if (enemy.IsDie())
        {
            state = State.BattleEnd;
            //EnemyDelete();
        }
        else if (player.IsDie())
        {
            state = State.BattleEnd;
            //PlayerRespawn();
        }

        Debug.Log(
            "PlayerHP : " + player.GetHp() + "/" + player.maxHp +
            " EnemyHP : " + enemy.GetHp() + "/" + enemy.maxHp
            );
        changeAttackOrDefense();
    }
}
