﻿using UnityEngine;
using System.Collections.Generic;

public class BattleUtil
{
    public static BattlePlayer GetPlayer(Unit unit, BattlePlayerUI ui)
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

        int bounsStat = 0;

        return new BattlePlayer(attackDices, defenseDices,
                maxHp, currentHp, bounsStat, ui);
    }

    public static BattlePlayer GetPlayer(Character unit, BattlePlayerUI ui)
    {
        if (unit.GetBonusStat() != 0)
        {
            Debug.Log("BounsStat : " + unit.GetBonusStat());
        }

        List<BDice.Species> attackDices = new List<BDice.Species>();
        List<BDice.Species> defenseDices = new List<BDice.Species>();
        
        for(int i = 0; i < unit.numberOfAttackDice + unit.GetBonusStat(); i++)
        {
            attackDices.Add(unit.speciesOfAttackDice);
        }
        
        for(int i = 0; i < unit.numberOfDefenseDice + unit.GetBonusStat(); i++)
        {
            defenseDices.Add(unit.speciesOfDefenseDice);
        }
        
        int maxHp = unit.maxHp;
        int currentHp = unit.currentHp;

        int bonusStat = unit.GetBonusStat();
        
        return new BattlePlayer(attackDices, defenseDices,
                                maxHp, currentHp, bonusStat, ui);
    }
}
