using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
	public static System.Random random = new System.Random();

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

    void SetBattleUnitImg(Sprite leftUnitImg, Sprite rightUnitImg)
    {
        leftPlayerUI.unitRenderer.sprite = leftUnitImg;
        rightPlayerUI.unitRenderer.sprite = rightUnitImg;
    }

	public void ShowBattle(CharacterManager playerManager, EnemyManager enemyManager, bool isMine, AttackOrDefense attackOrDefense)
	{
		battleCamera.enabled = true;
		state = State.Start;
		this.attackOrDefense = attackOrDefense;

        Sprite playerImg = playerManager.GetCharacterInstance().charImg;
        Sprite enemyImg = enemyManager.GetEnemyInstance().enemySprite;

		if (attackOrDefense == AttackOrDefense.Attack)
		{
			player = BattleUtil.GetPlayer(playerManager.GetCharacterInstance(), leftPlayerUI);
            enemy = BattleUtil.GetPlayer(enemyManager.GetEnemyInstance(), rightPlayerUI);

            SetBattleUnitImg(playerImg, enemyImg);

			enemy.SwitchDice();
		}
		else
		{
			player = BattleUtil.GetPlayer(playerManager.GetCharacterInstance(), rightPlayerUI);
			enemy = BattleUtil.GetPlayer(enemyManager.GetEnemyInstance(), leftPlayerUI);
            
            SetBattleUnitImg(enemyImg, playerImg);
       
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
		player.ResetDiceTransform();
		enemy.ResetDiceTransform();
	}

	Run ChangeAttackOrDefense()
	{
		if (attackOrDefense == AttackOrDefense.Attack)
		{
			attackOrDefense = AttackOrDefense.Defense;
		}
		else
		{
			attackOrDefense = AttackOrDefense.Attack;
		}

		var playerSwitch = player.SwitchDice();
		var enemySwitch = enemy.SwitchDice();

		return Run.Join(new List<Run>{ playerSwitch, enemySwitch });
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

		GameObject animationGameObject = null;
		if (attackOrDefense == AttackOrDefense.Attack)
		{
			for (int i = 0; i < playerDiceNum; i++)
			{
				int diceResult = playerCalcResult.diceResults[i];
				player.ui.attackDices[i].SendMessage("rollByNumber", diceResult);
				animationGameObject = player.ui.attackDices[i];
			}

			for (int i = 0; i < enemyDiceNum; i++)
			{
				int diceResult = enemyCalcResult.diceResults[i];
				enemy.ui.defenseDices[i].SendMessage("rollByNumber", diceResult);
				animationGameObject = enemy.ui.defenseDices[i];
			}
		}
		else
		{
			for (int i = 0; i < playerDiceNum; i++)
			{
				int diceResult = playerCalcResult.diceResults[i];
				player.ui.defenseDices[i].SendMessage("rollByNumber", diceResult);
				animationGameObject = player.ui.defenseDices[i];
			}

			for (int i = 0; i < enemyDiceNum; i++)
			{
				int diceResult = enemyCalcResult.diceResults[i];
				enemy.ui.attackDices[i].SendMessage("rollByNumber", diceResult);
				animationGameObject = enemy.ui.attackDices[i];
			}
		}

		var diceAnimation = animationGameObject.GetComponent<DiceAnimation>();
		Run.After(0.1f, () => {
			Run.WaitWhile(diceAnimation.IsRollAnimating)
				.ExecuteWhenDone(() => {
					int totalPlayerDice = playerCalcResult.totalDiceResult;
					int totalEnemyDice = enemyCalcResult.totalDiceResult;
					//show animation with calculation result.
					state = State.ShowDamage;
					Run.Coroutine(AnimateDamage(totalPlayerDice, totalEnemyDice));
			});
		});
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

	IEnumerator AnimateDamage(int totalPlayerDice, int totalEnemyDice)
	{
		BattlePlayer target = null;
		int damage = 0;

		damage = CalculateDamage(totalPlayerDice, totalEnemyDice);
		target = CompareDamageAndSelectTarget(totalPlayerDice, totalEnemyDice);

		Debug.Log("PlayerDice : " + totalPlayerDice + ", EnemyDice : " + totalEnemyDice);
		//show animation with calculation result.
		//apply damage.
		MultiAudioClip multiAudioClip = GetComponent<MultiAudioClip>();

		yield return new WaitForSeconds(DelayManager.Get().battleDiceResultToAttackDelay);

		if (damage > target.GetHp())
		{
			damage = target.GetHp();
		}

		if (target != null)
		{
			for(int i=1; i<=damage; i++)
			{
				multiAudioClip.audioSources[0].Play ();
				UpdateRemainHP();
				yield return new WaitForSeconds(DelayManager.Get().battleHpMinusDelay);
				target.ApplyDamage (1);
			}
		}
		else
		{
			multiAudioClip.audioSources[1].Play ();
			UpdateRemainHP();
			yield return new WaitForSeconds(DelayManager.Get().battleHpMinusDelay);
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

		var changeAnimation = ChangeAttackOrDefense();
		changeAnimation.ExecuteWhenDone(() => {
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
			else
			{
				state = State.WaitingRoll;
			}

			Debug.Log(
					"PlayerHP : " + player.GetHp() + "/" + player.maxHp +
					" EnemyHP : " + enemy.GetHp() + "/" + enemy.maxHp
					);
		});
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
