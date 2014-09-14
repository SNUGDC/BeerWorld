using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
	public enum State
	{
		Inactive,
		Start,
		WaitingRoll,
		ShowRoll,
		ShowDamage,
		BattleEnd
	}

	public enum AttackOrDefense
	{
		Attack,
		Defense
	}

	public static BattleManager battleManagerInstance = null;

	void Awake()
	{
		battleManagerInstance = this;
	}

	public State GetBattleState()
	{
		return state;
	}

	public BattlePlayerUI leftPlayerUI;
	public BattlePlayerUI rightPlayerUI;

	State state = State.Inactive;
	AttackOrDefense attackOrDefense = AttackOrDefense.Attack;
	BattlePlayer player;
	BattlePlayer enemy;
	CalculationResult playerCalcResult = null;
	CalculationResult enemyCalcResult = null;
	public Camera battleCamera;
	public bool isMine {
		get;
		private set;
	}

	private CharacterManager playerManager;
	private EnemyManager enemyManager;
	public void ShowBattle(CharacterManager playerManager, EnemyManager enemyManager, bool isMine, AttackOrDefense attackOrDefense)
	{
		battleCamera.enabled = true;
		state = State.Start;
		this.attackOrDefense = attackOrDefense;

		if (attackOrDefense == AttackOrDefense.Attack)
		{
			player = BattleUtil.GetPlayer(playerManager.GetCharacterInstance(), leftPlayerUI);
			enemy = BattleUtil.GetPlayer(enemyManager.GetUnitInstance(), rightPlayerUI);
			enemy.SwitchDice();
		}
		else
		{
			player = BattleUtil.GetPlayer(playerManager.GetCharacterInstance(), rightPlayerUI);
			enemy = BattleUtil.GetPlayer(enemyManager.GetUnitInstance(), leftPlayerUI);
			player.SwitchDice();
		}

		this.playerManager = playerManager;
		this.enemyManager = enemyManager;
		this.isMine = isMine;
	}

	public void EndBattle()
	{
		BattleResultApplier.ApplyBattleResult(player, enemy, playerManager, enemyManager);
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

		player.SwitchDice();
		enemy.SwitchDice();
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
			state = State.WaitingRoll;

			UpdateRemainHP();
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
				player.ui.attackDices[i].SendMessage("rollByNumber", diceResult);
			}

			for (int i = 0; i < enemyDiceNum; i++)
			{
				int diceResult = enemyCalcResult.diceResults[i];
				enemy.ui.defenseDices[i].SendMessage("rollByNumber", diceResult);
			}
		}
		else
		{
			for (int i = 0; i < playerDiceNum; i++)
			{
				int diceResult = playerCalcResult.diceResults[i];
				player.ui.defenseDices[i].SendMessage("rollByNumber", diceResult);
			}

			for (int i = 0; i < enemyDiceNum; i++)
			{
				int diceResult = enemyCalcResult.diceResults[i];
				enemy.ui.attackDices[i].SendMessage("rollByNumber", diceResult);
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
		else
		{
			player.ApplyDamage(1);
			enemy.ApplyDamage(1);
			Debug.Log("Each player is Damaged 1");
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
			BattleResultApplier.state = BattleResultApplier.BattleResultState.PlayerWin;
			state = State.BattleEnd;
		}
		else if (player.IsDie())
		{
			BattleResultApplier.state = BattleResultApplier.BattleResultState.EnemyWin;
			state = State.BattleEnd;
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

		for (int i = 0; i < player.ui.hearts.Length; i++)
		{
			if (remainPlayerHPRatio <= ((float)i / (float)player.ui.hearts.Length))
			{
				player.ui.hearts[i].SetActive(false);
			}
			else
			{
				player.ui.hearts[i].SetActive(true);
			}
		}

		for (int i = 0; i < enemy.ui.hearts.Length; i++)
		{
			if (remainEnemyHPRatio <= ((float)i / (float)enemy.ui.hearts.Length))
			{
				enemy.ui.hearts[i].SetActive(false);
			}
			else
			{
				enemy.ui.hearts[i].SetActive(true);
			}
		}
	}
}
