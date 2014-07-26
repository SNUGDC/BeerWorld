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

    public static BattlePlayer GetPlayer(Character character)
    {
        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();

        for(int i = 0; i < character.numberOfAttackDice; i++)
        {
            attackDices.Add(character.speciesOfAttackDice); 
        }

        for(int i = 0; i < character.numberOfDefenseDice; i++)
        {
            defenseDices.Add(character.speciesOfDefenseDice);    
        }    

        int maxHp = character.maxHp;
        int currentHp = character.currentHp;

        return new BattlePlayer(attackDices, defenseDices,
                maxHp, currentHp);   
    }
}
