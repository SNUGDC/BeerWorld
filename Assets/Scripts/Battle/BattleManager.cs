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

    public GameObject[] playerHearts = new GameObject[4];
    public GameObject[] enemyHearts = new GameObject[4];

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

    void ChangeAttackOrDefense()
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
            Debug.Log("BattleEnd.");
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

        int totalPlayerDice = playerCalcResult.totalDiceResult;
        int totalEnemyDice = enemyCalcResult.totalDiceResult;
        //show animation with calculation result.
        state = State.ShowDamage;
        AnimateDamage(totalPlayerDice, totalEnemyDice);
    }

    BattlePlayer CompareDamageAndSelectTarget(int totalPlayerDice, int totalEnemyDice)
    {
        if (totalPlayerDice > totalEnemyDice)
        {
            return enemy;
        }
        else if (totalPlayerDice < totalEnemyDice) 
        {
            return player;
        }
        else
        {
            Debug.Log("Dice is same.");
            return null;
        }
    }

    int CalculateDamage(int totalPlayerDice, int totalEnemyDice)
    {
        return System.Math.Abs(totalPlayerDice - totalEnemyDice);
    }

    void AnimateDamage(int totalPlayerDice, int totalEnemyDice)
    {
        BattlePlayer target = null;
        int damage = 0;
        
//        CompareDamageAndSelectTarget(totalPlayerDice, totalEnemyDice);
        damage = CalculateDamage(totalPlayerDice, totalEnemyDice);
        target = CompareDamageAndSelectTarget(totalPlayerDice, totalEnemyDice);

        Debug.Log("PlayerDice : " + totalPlayerDice + ", EnemyDice : " + totalEnemyDice);
        //show animation with calculation result.
        //apply damage.
        state = State.WaitingRoll;
        
        if (target != null)
        {
            target.ApplyDamage(damage);
        }

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

        UpdateRemainHP();

        Debug.Log(
            "PlayerHP : " + player.GetHp() + "/" + player.maxHp +
            " EnemyHP : " + enemy.GetHp() + "/" + enemy.maxHp
            );
        
        ChangeAttackOrDefense();
    }

    void UpdateRemainHP()
    {
        float remainPlayerHPRatio = (float)player.GetHp() / (float)player.maxHp;
        float remainEnemyHPRatio = (float)enemy.GetHp() / (float)enemy.maxHp;

        Debug.Log(
            "PlayerHP ratio : " + remainPlayerHPRatio +
            " EnemyHP ratio : " + remainEnemyHPRatio
            );

//        ShowUserHP(remainPlayerHPRatio * playerHearts.Length);
        for (int i = 0; i < playerHearts.Length; i++)
        {
            if (remainPlayerHPRatio <= ((float)i / (float)playerHearts.Length))
            {
                playerHearts[i].SetActive(false);
            }
        }

        for (int i = 0; i < enemyHearts.Length; i++)
        {
            if (remainEnemyHPRatio <= ((float)i / (float)enemyHearts.Length))
            {
                enemyHearts[i].SetActive(false);
            }
        }
    }

    void ShowUserHP(int life)
    {
        if (life < 0 || playerHearts.Length < life)
        {
            Debug.LogError("Life not valid.");
        }

        for (int i=0; i < playerHearts.Length; i++)
        {
            if (i < life)
            {
                playerHearts[i].SetActive(true);
            }
            else
            {
                playerHearts[i].SetActive(false);
            }
        }
    }
}
