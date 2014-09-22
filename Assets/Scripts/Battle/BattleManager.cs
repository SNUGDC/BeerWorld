using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Slinq;

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

	public static BattleManager Get()
	{
		return battleManagerInstance;
	}

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

    int totalPlayerDice = 0;
    int totalEnemyDice = 0;

//--Item Trigger
    List<Character.Item> useItemsInBattle = new List<Character.Item>();

	public bool IsPlayerTurn(CharacterManager characterManager)
	{
		return characterManager == playerManager;
	}

		void UpdateBuffUI()
		{
			player.DisableAllBuffUI();
			useItemsInBattle.ForEach((item) => {
				player.EnableBuffUI(item);
			});
		}

    public void AddUseItemInBattle(Character.Item item)
    {
        useItemsInBattle.Add(item);
				UpdateBuffUI();
    }

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
//------Berserk
		if (useItemsInBattle.Contains(Character.Item.Berserk))
        {
            player.CopyDefenceDicesToAttackDices();
            useItemsInBattle.Remove(Character.Item.Berserk);
        }
//------Block
        if (useItemsInBattle.Contains(Character.Item.Block))
        {
            player.CopyDefenceDicesToAttackDices();
            useItemsInBattle.Remove(Character.Item.Block);
        }
//------Adding
        if (useItemsInBattle.Contains(Character.Item.Adding))
        {
            player.AddBonusStat();
            useItemsInBattle.Remove(Character.Item.Adding);
        }
        
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
			enemyCalcResult = calculator.GetAttackDiceResult(enemy);
		}

		Run.After(DelayManager.Get().battleDiceRollToDiceResultDelay, () => {
			state = State.ShowRoll;
			AnimateDice();
		});
	}

	void AnimateDice()
	{
		int playerDiceNum = playerCalcResult.diceResults.Count;
		int enemyDiceNum = enemyCalcResult.diceResults.Count;

		GameObject animationGameObject = null;

		Action<List<int>, GameObject[]> render = (diceResults, uiDices) => {
			Queue<int> diceResultQueue = new Queue<int>(diceResults);
			Slinqable.Slinq(uiDices)
				.Reverse()
				.Take(diceResults.Count)
				.ForEach((attackDice) => {
					var diceResult = diceResultQueue.Dequeue();
					attackDice.SendMessage("rollByNumber", diceResult);
					animationGameObject = attackDice;
				});
		};

		if (attackOrDefense == AttackOrDefense.Attack)
		{
			render(playerCalcResult.diceResults, player.ui.attackDices);
			render(enemyCalcResult.diceResults, enemy.ui.defenseDices);
		}
		else
		{
			render(playerCalcResult.diceResults, player.ui.defenseDices);
			render(enemyCalcResult.diceResults, enemy.ui.attackDices);
		}

		//Wait for animation end.
		var diceAnimation = animationGameObject.GetComponent<DiceAnimation>();
		Run.WaitSeconds(0.1f)
		.Then(Run.WaitWhile(diceAnimation.IsRollAnimating))
		.ExecuteWhenDone(() => {
			totalPlayerDice = playerCalcResult.totalDiceResult;
			totalEnemyDice = enemyCalcResult.totalDiceResult;

			player.SetTotalDiceResult(totalPlayerDice);
			enemy.SetTotalDiceResult(totalEnemyDice);

			Run useItem = Run.WaitSeconds(0);
//------------------DiceChange
			if (useItemsInBattle.Contains(Character.Item.DiceChange) == true)
			{
				ChangeDiceWithEnemy();
				useItemsInBattle.Remove(Character.Item.DiceChange);
				UpdateBuffUI();
			}

//------------------DiceResultChange
			if (useItemsInBattle.Contains(Character.Item.DiceResultChange) == true)
			{
				useItem.Then(() => {
						return DiceReroll().ExecuteWhenDone(() => {
							useItemsInBattle.Remove(Character.Item.DiceResultChange);
							UpdateBuffUI();
						});
					}
				);
			}

			useItem.ExecuteWhenDone(() => {
				//show animation with calculation result.
				totalPlayerDice = playerCalcResult.totalDiceResult;
				totalEnemyDice = enemyCalcResult.totalDiceResult;

				player.SetTotalDiceResult(totalPlayerDice);
				enemy.SetTotalDiceResult(totalEnemyDice);

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

    BattlePlayer target = null;
    int damage = 0;

    IEnumerator AnimateDamage(int totalPlayerDice, int totalEnemyDice)
	{
		damage = CalculateDamage(totalPlayerDice, totalEnemyDice);
		target = CompareDamageAndSelectTarget(totalPlayerDice, totalEnemyDice);

		Debug.Log("PlayerDice : " + totalPlayerDice + ", EnemyDice : " + totalEnemyDice);
		//show animation with calculation result.
		//apply damage.
		MultiAudioClip multiAudioClip = GetComponent<MultiAudioClip>();
	
		yield return new WaitForSeconds(DelayManager.Get().battleDiceResultToAttackDelay);

		if (target != null)
		{
//----------Dodge item.
			if (useItemsInBattle.Contains(Character.Item.Dodge))
			{
				Dodge();
				useItemsInBattle.Remove(Character.Item.Dodge);
			}

			if (damage > target.GetHp())
			{
				damage = target.GetHp();
			}

			for(int i=1; i<=damage; i++)
			{
				target.ApplyDamage (1);
				UpdateRemainHP();
				if(target.ui == leftPlayerUI)
					rightPlayerUI.unitRenderer.transform.parent.gameObject.SendMessage("Attack");
				else if(target.ui == rightPlayerUI)
					leftPlayerUI.unitRenderer.transform.parent.gameObject.SendMessage("Attack");
				target.ui.unitRenderer.transform.parent.gameObject.SendMessage("Hit");
				multiAudioClip.audioSources[0].Play ();
				yield return new WaitForSeconds(DelayManager.Get().battleHpMinusDelay);
			}
		}
		else
		{
			multiAudioClip.audioSources[1].Play ();
			if (useItemsInBattle.Contains(Character.Item.Dodge))
			{
				Dodge();
				useItemsInBattle.Remove(Character.Item.Dodge);
			}
			else
			{
				player.ApplyDamage(1);
				player.ui.unitRenderer.transform.parent.gameObject.SendMessage("Hit");
			}
 			enemy.ApplyDamage(1);
			UpdateRemainHP();
			enemy.ui.unitRenderer.transform.parent.gameObject.SendMessage("Hit");
			yield return new WaitForSeconds(DelayManager.Get().battleHpMinusDelay);
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
			enemy.ui.unitRenderer.transform.parent.gameObject.SendMessage("Fade");
			BattleResultApplier.state = BattleResultApplier.BattleResultState.PlayerWin;
			yield return new WaitForSeconds(DelayManager.Get().batttleLoseShowDelay);
			state = State.BattleEnd;
		}
		else if (player.IsDie())
		{
			player.ui.unitRenderer.transform.parent.gameObject.SendMessage("Fade");
			BattleResultApplier.state = BattleResultApplier.BattleResultState.EnemyWin;
			yield return new WaitForSeconds(DelayManager.Get().batttleLoseShowDelay);
			state = State.BattleEnd;
		}
		else
		{
			var changeAnimation = ChangeAttackOrDefense();
			changeAnimation.ExecuteWhenDone(() => {
				state = State.WaitingRoll;
				Debug.Log(
						"PlayerHP : " + player.GetHp() + "/" + player.maxHp +
						" EnemyHP : " + enemy.GetHp() + "/" + enemy.maxHp
						);
			});
		}
	}

	void UpdateRemainHP()
	{
		for (int i = 0; i < player.ui.hearts.Length; i++)
		{
			if (i < player.GetHp())
			{
				player.ui.hearts[i].SetActive(true);
			}
			else
			{
				player.ui.hearts[i].SetActive(false);
			}
		}

		for (int i = 0; i < enemy.ui.hearts.Length; i++)
		{
			if (i < enemy.GetHp())
			{
				enemy.ui.hearts[i].SetActive(true);
			}
			else
			{
				enemy.ui.hearts[i].SetActive(false);
			}
		}
	}

    void ChangeDiceWithEnemy()
    {
        //Add DiceChange effect.
			// FIXME: Is diceResults also swapped?
        int temp = playerCalcResult.totalDiceResult;
        playerCalcResult.totalDiceResult = enemyCalcResult.totalDiceResult;
        enemyCalcResult.totalDiceResult = temp;

        Debug.Log("Changed player diceResult and enemy diceResult");
    }

    Run DiceReroll()
    {
        //Add effect.

        int minDiceValue = Slinqable.Slinq(playerCalcResult.diceResults).Min();
        int indexOfLowestDice = playerCalcResult.diceResults.FindIndex(
            (diceResult) => diceResult == minDiceValue);


        if (attackOrDefense == AttackOrDefense.Attack)
        {        
            //Add re-roll effect playerDice @player.ui.attackDices[i]

            int diceResult = playerCalcResult.diceResults [indexOfLowestDice];
            player.ui.attackDices [indexOfLowestDice].SendMessage("rollByNumber", diceResult);

			var animationGameObject = enemy.ui.attackDices[indexOfLowestDice];
			var diceAnimation = animationGameObject.GetComponent<DiceAnimation>();

            totalPlayerDice += (diceResult - minDiceValue);

			return Run.WaitSeconds(0.1f).Then(Run.WaitWhile(diceAnimation.IsRollAnimating));
        }
		else
		{
			return Run.WaitSeconds(0);
		}
    }
        
    void Dodge()
    {
        //Add dodge effect.
        if (target == player)
        {
            damage = 0;
        } 
        else if (target == null)
        {
            player.ApplyDamage(0);
        }
    }
        
    void Berserk()
    {
    }

    void Block()
    {
    }

    void Adding()
    {
    }
}

