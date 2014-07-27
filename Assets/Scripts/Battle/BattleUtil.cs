using UnityEngine;
using System.Collections.Generic;

public class BattleUtil
{
    public static BattlePlayer GetDummyPlayer()
    {
        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();

        attackDices.Add(BDice.Species.Six);
        attackDices.Add(BDice.Species.Six);
        attackDices.Add(BDice.Species.Six);

        defenseDices.Add(BDice.Species.Six);

        return new BattlePlayer(attackDices, defenseDices,
                10, 10);
    }

    public static BattlePlayer GetDummyEnemy()
    {
        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();

        attackDices.Add(BDice.Species.Six);
        attackDices.Add(BDice.Species.Six);

        defenseDices.Add(BDice.Species.Six);
        defenseDices.Add(BDice.Species.Six);

        return new BattlePlayer(attackDices, defenseDices,
                5, 5);
    }

    public static BattlePlayer GetPlayer(Unit unit)
    {
        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();

        for(int i = 0; i < unit.numberOfAttackDice; i++)
        {
            attackDices.Add(unit.speciesOfAttackDice);
        }

        for(int i = 0; i < unit.numberOfDefenseDice; i++)
        {
            defenseDices.Add(unit.speciesOfDefenseDice);
        }

        int maxHp = unit.maxHp;
        int currentHp = unit.currentHp;

        return new BattlePlayer(attackDices, defenseDices,
                maxHp, currentHp);
    }
}
