using UnityEngine;
using System.Collections.Generic;

public class BDice
{
  public enum Species
  {
    Four,
    Six
  }
}

[System.Serializable]
public class BattlePlayerUI
{
  public GameObject[] attackDices = new GameObject[3];
  public GameObject[] defenseDices = new GameObject[3];
  public GameObject[] hearts = new GameObject[4];
}

public class BattlePlayer
{
  public readonly List<BDice.Species> attackDices;
  public readonly List<BDice.Species> defenseDices;
  public readonly int maxHp;
  public readonly BattlePlayerUI ui;
  private int currentHp;

  //using Test.
  public int GetHp()
  {
    return currentHp;
  }

  public BattlePlayer(List<BDice.Species> attackDices,
      List<BDice.Species> defenseDices,
      int maxHp, int currentHp,
      BattlePlayerUI ui)
  {
    this.attackDices = attackDices;
    this.defenseDices = defenseDices;
    this.maxHp = maxHp;
    this.ui = ui;
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
}
