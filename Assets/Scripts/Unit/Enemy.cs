using UnityEngine;
using System.Collections;

public class Enemy : Unit
{
	public enum EnemyType
    {
        Smallest,
        Middle,
        None
    }

    public enum EnemyColor
    {
        Red,
        White,
        None
    }

    public Sprite redMiddle;
    public Sprite whiteMiddle;
    public Sprite redSmallest;
    public Sprite whiteSmallest;

    public Sprite enemySprite;

    public SpriteRenderer renderer;

    EnemyType enemyType;
    EnemyColor enemyColor;

    public void SetEnemyType(EnemyType enemyType)
	{
		this.enemyType = enemyType;
	}

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }

	void SetSmallestEnemyStats()
	{
		Debug.Log("Set Values : Smallest");
		
        //FIXME : temporary color.
        enemyColor = EnemyColor.Red;
        enemySprite = redSmallest;

		maxHp = 2;
		currentHp = maxHp;

		numberOfAttackDice = 2;
		speciesOfAttackDice = BDice.Species.Four;

		numberOfDefenseDice = 2;
		speciesOfDefenseDice = BDice.Species.Four;

		numberOfMoveDice = 1;
		speciesOfMoveDice = BDice.Species.One;
	}

    void SetMiddleEnemyStats()
    {
        Debug.Log("Set Values : Middle");

        //FIXME : temporary color.
        enemyColor = EnemyColor.Red;
        enemySprite = redMiddle;

        maxHp = 3;
        currentHp = maxHp;
        
        numberOfAttackDice = 3;
        speciesOfAttackDice = BDice.Species.Four;
        
        numberOfDefenseDice = 3;
        speciesOfDefenseDice = BDice.Species.Four;
        
        numberOfMoveDice = 0;
        speciesOfMoveDice = BDice.Species.Six;
    }

	void Start ()
	{
		if (enemyType == EnemyType.Smallest)
        {
            SetSmallestEnemyStats();

            if (enemyColor == EnemyColor.Red)
            {
                enemySprite = redSmallest;
            }
            else if (enemyColor == EnemyColor.White)
            {
                enemySprite = whiteSmallest;
            }
        } 
        else if (enemyType == EnemyType.Middle)
        {
            SetMiddleEnemyStats();

            if (enemyColor == EnemyColor.Red)
            {
                enemySprite = redMiddle;
            }
            else if (enemyColor == EnemyColor.White)
            {
                enemySprite = whiteMiddle;
            }
        }

        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = enemySprite;
	}
}
