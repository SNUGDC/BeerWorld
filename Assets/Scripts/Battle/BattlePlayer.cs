using UnityEngine;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

public class BDice
{
  public enum Species
  {
    One,
    Four,
    Six
  }
}

[System.Serializable]
public class BattleBuffUI
{
	public Character.Item item;
	public SpriteRenderer spriteRenderer;
}

[System.Serializable]
public class BattlePlayerUI
{
	public GameObject attackDiceParent;
    public GameObject[] attackDices = new GameObject[4];
	public GameObject defenseDiceParent;
    public GameObject[] defenseDices = new GameObject[4];
    public GameObject[] hearts = new GameObject[4];
    public SpriteRenderer unitRenderer;
	public List<BattleBuffUI> battleBuffUIs = new List<BattleBuffUI>();
	public TextMesh damageCount;
}

public class BattlePlayer
{
  public List<BDice.Species> attackDices;
  public List<BDice.Species> defenseDices;
  public readonly int maxHp;
  public readonly BattlePlayerUI ui;
  public int bonusStat;
  private int currentHp;

  //using Test.
  public int GetHp()
  {
    return currentHp;
  }

  public BattlePlayer(List<BDice.Species> attackDices,
      List<BDice.Species> defenseDices,
      int maxHp, int currentHp, int bonusStat,
      BattlePlayerUI ui)
  {
    this.attackDices = attackDices;
    this.defenseDices = defenseDices;
    this.maxHp = maxHp;
    this.ui = ui;
    this.bonusStat = bonusStat;
    this.currentHp = currentHp;

		ui.battleBuffUIs.ForEach((buffUI) => {
			buffUI.spriteRenderer.enabled = false;
		});

		InitializeDice();

		ui.damageCount.text = "0";
  }

    public void CopyAttackDicesToDefenceDices()
    {
        this.defenseDices = this.attackDices;
        //animation.
        //re-show defenctDices.
    }

    public void CopyDefenceDicesToAttackDices()
    {
        this.attackDices = this.defenseDices;
        //animation.
        //re-show attackDices.
    }

    public void AddBonusStat()
    {
        int newBonusStat = Random.Range(1, 4);
        this.bonusStat += newBonusStat;
        //animation.
    }

	void InitializeDice()
	{
		Slinqable.Slinq(ui.attackDices)
			.ForEach((attackDice) => {
				attackDice.SetActive(false);
			});

		Queue<BDice.Species> attackDiceSpecies = new Queue<BDice.Species>(attackDices);
		Slinqable.Slinq(ui.attackDices)
			.Reverse()
			.Take(attackDices.Count)
			.ForEach((attackDice) => {
				attackDice.SetActive(true);
				if (attackDiceSpecies.Dequeue() == BDice.Species.Four)
				{
					attackDice.SendMessage("roll4ByNumber", 1);
				}
				else
				{
					attackDice.SendMessage("rollByNumber", 1);
				}
			});

		Slinqable.Slinq(ui.defenseDices)
			.ForEach((defenseDice) => {
				defenseDice.SetActive(false);
			});

		Queue<BDice.Species> defenseDiceSpecies = new Queue<BDice.Species>(defenseDices);
		Slinqable.Slinq(ui.defenseDices)
			.Reverse()
			.Take(defenseDices.Count)
			.ForEach((defenseDice) => {
				defenseDice.SetActive(true);
				if (defenseDiceSpecies.Dequeue() == BDice.Species.Four)
				{
					defenseDice.SendMessage("roll4ByNumber", 1);
				}
				else
				{
					defenseDice.SendMessage("rollByNumber", 1);
				}
			});
	}

  public void ApplyDamage(int damage)
  {
    currentHp -= damage;
  }

  public bool IsLive()
  {
    return currentHp > 0;
  }

  public bool IsDie()
  {
    return IsLive() == false;
  }

	public Run SwitchDice()
	{
		var attackDicePosition = ui.attackDiceParent.transform.position;
		var attackDiceScale = ui.attackDiceParent.transform.localScale;

		var defenseDicePosition = ui.defenseDiceParent.transform.position;
		var defenseDiceScale = ui.defenseDiceParent.transform.localScale;

		var attackLerp = Run.Lerp(0.3f, (ratio) => {
			ui.attackDiceParent.transform.position = Vector3.Slerp(attackDicePosition, defenseDicePosition, ratio);
			ui.attackDiceParent.transform.localScale = Vector3.Slerp(attackDiceScale, defenseDiceScale, ratio);
		});

		var defenseLerp = Run.Lerp(0.3f, (ratio) => {
			ui.defenseDiceParent.transform.position = Vector3.Slerp(defenseDicePosition, attackDicePosition, ratio);
			ui.defenseDiceParent.transform.localScale = Vector3.Slerp(defenseDiceScale, attackDiceScale, ratio);
		});

		return Run.Join(new List<Run>{ attackLerp, defenseLerp });
	}

	public void ResetDiceTransform()
	{
		Debug.LogWarning("Reset dice transform");
		var attackDicePosition = ui.attackDiceParent.transform.position;
		var attackDiceScale = ui.attackDiceParent.transform.localScale;

		var defenseDicePosition = ui.defenseDiceParent.transform.position;
		var defenseDiceScale = ui.defenseDiceParent.transform.localScale;

		if (attackDicePosition.y < defenseDicePosition.y)
		{
			Debug.Log("Change attack and defense in reset dice");
			ui.attackDiceParent.transform.position = defenseDicePosition;
			ui.attackDiceParent.transform.localScale = defenseDiceScale;
			ui.defenseDiceParent.transform.position = attackDicePosition;
			ui.defenseDiceParent.transform.localScale = attackDiceScale;
		}
	}

	public void DisableAllBuffUI()
	{
		ui.battleBuffUIs.ForEach((buffUI) => {
				buffUI.spriteRenderer.enabled = false;
		});
	}

	public void EnableBuffUI(Character.Item item)
	{
		Slinqable.Slinq(ui.battleBuffUIs)
			.Where((buffUI) => buffUI.item == item)
			.ForEach((buffUI) => buffUI.spriteRenderer.enabled = true);
	}

	public Run ScaleBuffUI(Character.Item item)
	{
		float scale = 1.5f;
		float effectTime = 0.5f;

		Vector3 scaleVector = new Vector3(scale, scale, 1);

		Option<Run> effect = Slinqable.Slinq(ui.battleBuffUIs)
			.Where((buffUI) => buffUI.item == item)
			.FirstOrNone()
			.Select((buffUI) => {
				var buffGO = buffUI.spriteRenderer.gameObject;
				iTween.ScaleTo(buffGO, iTween.Hash("scale", scaleVector, "time", effectTime));
				return Run.WaitSeconds(effectTime)
					.ExecuteWhenDone(() => {
						iTween.ScaleTo(buffGO, iTween.Hash("scale", Vector3.one, "time", effectTime));
					})
					.Then(() => Run.WaitSeconds(effectTime));
			});

		return effect.ValueOr(Run.WaitSeconds(0));
	}

	public void SetTotalDiceResult(int diceResult)
	{
		ui.damageCount.text = diceResult.ToString();
	}
}
