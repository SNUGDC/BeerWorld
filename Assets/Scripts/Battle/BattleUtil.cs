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

    public static BattlePlayer GetPlayer(Character player)
    {
        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();

        for(int i = 0; i < player.numberOfAttackDice; i++)
        {
            attackDices.Add(player.speciesOfAttackDice); 
        }

        for(int i = 0; i < player.numberOfDefenseDice; i++)
        {
            defenseDices.Add(player.speciesOfDefenseDice);    
        }    

        int maxHp = player.maxHp;
        int currentHp = player.currentHp;

        return new BattlePlayer(attackDices, defenseDices,
                maxHp, currentHp);   
    }
}
