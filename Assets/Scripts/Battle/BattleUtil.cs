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
                10);
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
                5);
    }

    public static BattlePlayer GetPlayer(Character Player)
    {
        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();

        attackDices.Add(BDice.Species.Six);
        attackDices.Add(BDice.Species.Six);
        attackDices.Add(BDice.Species.Six);

        defenseDices.Add(BDice.Species.Six);

        return new BattlePlayer(attackDices, defenseDices,
                10);   
    }
}
