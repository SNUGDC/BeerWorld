using UnityEngine;
using System.Collections.Generic;

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
public class BattlePlayerUI
{
	public GameObject attackDiceParent;
    public GameObject[] attackDices = new GameObject[3];
	public GameObject defenseDiceParent;
    public GameObject[] defenseDices = new GameObject[3];
    public GameObject[] hearts = new GameObject[4];
    public SpriteRenderer unitRenderer;
}

public class BattlePlayer
{
  public readonly List<BDice.Species> attackDices;
  public readonly List<BDice.Species> defenseDices;
  public readonly int maxHp;
  public readonly BattlePlayerUI ui;
  public readonly int bonusStat;
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
}
