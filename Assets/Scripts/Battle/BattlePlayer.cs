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

public class BattlePlayer
{
    public readonly List<BDice.Species> attackDices;
    public readonly List<BDice.Species> defenseDices;
    public readonly int maxHp;
    private int currentHp;

    //using Test.
    public int GetHp()
    {
        return currentHp;
    }

    public BattlePlayer(List<BDice.Species> attackDices,
            List<BDice.Species> defenseDices,
            int maxHp)
    {
        this.attackDices = attackDices;
        this.defenseDices = defenseDices;
        this.maxHp = maxHp;
        this.currentHp = maxHp;
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
