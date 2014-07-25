using UnityEngine;
using System.Collections;

public class BattleResult 
{
	public enum BattleResultState
	{
		PlayerWin,
		EnemyWin,
		Draw,
		Default
	}

	public static BattleResultState state = BattleResultState.Default;

	public static void EnemyDelete(BattlePlayer enemy)
	{

	}

	public static void PlayerRespawn(BattlePlayer player)
	{
		Character characterInstance = GameManager.GetMyCharacterManager().GetCharacterInstance();

		characterInstance.currentHp = characterInstance.maxHp;
		Debug.Log("currentHp : " + characterInstance.currentHp + " maxHp : " + characterInstance.maxHp);

		characterInstance.preTileKey = 000;
		characterInstance.prePreTileKey = 000;

		Tile startTile = TileManager.GetStartTile ();
        Vector3 startTilePosition = startTile.gameObject.transform.position;
        Vector3 startPositionOfCharacter = new Vector3(startTilePosition.x, startTilePosition.y, Character.Depth);

        characterInstance.transform.position = startPositionOfCharacter; 
        Vector2 characterCoordinate = FieldTileUtility.GetCoordFromPosition(startPositionOfCharacter.x, startPositionOfCharacter.y);
        characterInstance.currentTileKey = (int)(characterCoordinate.x * 100 + characterCoordinate.y);
        characterInstance.preTileKey = 000;
        characterInstance.prePreTileKey = 000;

        Camera.main.transform.position = new Vector3(startPositionOfCharacter.x, startPositionOfCharacter.y, Camera.main.transform.position.z);
	}

	public static void UpdateResult(BattlePlayer player, BattlePlayer enemy)
	{
		GameManager.GetMyCharacterManager().GetCharacterInstance().currentHp = player.GetHp();
		
		if((state == BattleResultState.PlayerWin)||(state == BattleResultState.Draw))
		{
			EnemyDelete(enemy);
			Debug.Log("Enemy is dead.");
		}
		if((state == BattleResultState.EnemyWin)||(state == BattleResultState.Draw))
		{
			PlayerRespawn(player);
			Debug.Log("Player return to startTile.");
		}

		state = BattleResultState.Default;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
