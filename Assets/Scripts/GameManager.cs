using UnityEngine;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Collections;
using Smooth.Slinq;
using Smooth;

public class GameManager : MonoBehaviour
{
	public static GameManager gameManagerInstance = null;
	public static GameManager Get()
	{
		return gameManagerInstance;
	}
	public Character characterPrefab;
	public DirectionArrow arrowPrefab;
	private CharacterManager myCharacterManager = null;
    private EnemyInfoHolder enemyInfoList = null;
	public Enemy enemyPrefab;
	private Dictionary<string, EnemyManager> enemies = new Dictionary<string, EnemyManager>();

    public GameObject gameOverImg;

    public static GameObject gameOverObj;

    public void PopGameOverImg()
    {
		foreach(KeyValuePair<NetworkViewID, CharacterManager> manager in otherCharacterManagers){
        	manager.Value.SetMoveStateInactive();
		}
		myCharacterManager.SetMoveStateInactive();
        gameOverImg.SetActive(true);
        Run.After(5f, () => {
            Application.LoadLevel("Login");
        });
    }

    public bool IsRemainMiddleEnemy()
    {
        EnemyManager enemy;
        foreach (KeyValuePair<string, EnemyManager> pair in enemies)
        {
            enemy = pair.Value;
            if (enemy.GetEnemyInstance().GetEnemyType() == Enemy.EnemyType.Middle)
            {
                return true;
            }
        }
        return false;
    }

	public Dictionary<string, EnemyManager> GetEnemies()
	{
		return enemies;
	}

	public IEnumerable<EnemyManager> GetEnemiesList()
	{
		return enemies.Values;
	}

    public IEnumerable<EnemyManager> GetEnemiesListSizeOrdered()
    {
        List<EnemyManager> enemiesListSizeOrdered = new List<EnemyManager>();
        EnemyManager enemy;
        foreach (KeyValuePair<string, EnemyManager> pair in enemies)
        {
            enemy = pair.Value;
            if (enemy.GetEnemyInstance().GetEnemyType() == Enemy.EnemyType.Smallest)
            {
                enemiesListSizeOrdered.Add(enemy);
            }
        }
        foreach (KeyValuePair<string, EnemyManager> pair in enemies)
        {
            enemy = pair.Value;
            if (enemy.GetEnemyInstance().GetEnemyType() == Enemy.EnemyType.Middle)
            {
                enemiesListSizeOrdered.Add(enemy);
            }
        }
        return enemiesListSizeOrdered;
    }

	public static CharacterManager GetMyCharacterManager()
	{
		return gameManagerInstance.myCharacterManager;
	}

	private Dictionary<NetworkViewID, CharacterManager> otherCharacterManagers = new Dictionary<NetworkViewID, CharacterManager>();

	private List<NetworkViewID> otherPlayers = new List<NetworkViewID>();
	private Dictionary<NetworkViewID, Character.CharClass> playerClasses = new Dictionary<NetworkViewID, Character.CharClass>();

	public static IEnumerable<CharacterManager> GetAllPlayersEnumerator()
	{
		yield return gameManagerInstance.myCharacterManager;

		foreach (var player in gameManagerInstance.otherCharacterManagers.Values)
		{
			yield return player;
		}
	}

	public static NetworkViewID GetNetworkViewID(CharacterManager player)
	{
		if (gameManagerInstance.isMyCharacterManager(player))
		{
			return NetworkManager.networkInstance.Id;
		}
		else
		{
			return Slinqable.Slinq(gameManagerInstance.otherPlayers).Where(
					(playerId) => {
					var otherPlayer = gameManagerInstance.otherCharacterManagers[playerId];
					return otherPlayer == player;
					}).First();
		}
	}

	public static CharacterManager GetCharacterManager(NetworkViewID id)
	{
		if (id.isMine)
		{
			return gameManagerInstance.myCharacterManager;
		}
		else
		{
			return gameManagerInstance.otherCharacterManagers[id];
		}
	}

	public bool isMyCharacterManager(CharacterManager unitManager)
	{
		return myCharacterManager == unitManager;
	}

	public void PassTurnToNextPlayer()
	{
		var waitingPassTurn = TurnManager.Get().PassTurn();
		waitingPassTurn.ExecuteWhenDone(() => {
			TurnManager.State turnState = TurnManager.Get().GetState();

			if (turnState == TurnManager.State.Player)
			{
				NetworkManager.SendTurnStartMessage(NetworkManager.networkInstance.Id);
			}
			else if (turnState == TurnManager.State.OtherPlayer)
			{
				NetworkViewID turnPlayerViewID = TurnManager.Get().GetTurnPlayer();
				NetworkManager.SendTurnStartMessage(turnPlayerViewID);
			}
			else if (turnState == TurnManager.State.Enemy)
			{
				NetworkManager.StartEnemyTurn();
				EnemyManager turnEnemy = TurnManager.Get().GetTurnEnemy();
				turnEnemy.ChangeMoveStateToIdle();
			}
		});
	}

	//Used for devtest.
	public void GameStartButtonClicked()
	{
		NetworkManager.SendUsersNetworkViewID();
	}

	public void AddUser(NetworkViewID id, Character.CharClass charClass)
	{
		if (id == NetworkManager.networkInstance.Id)
		{
			playerClasses.Add(id, charClass);

			//for localtest.
			if (Network.connections.Length == 0)
			{
				Run.After(0.3f, () => {
					GameStart();
				});
			}
			return;
		}

		otherPlayers.Add(id);
		playerClasses.Add(id, charClass);

		TurnManager.Get().AddPlayerTEMP(id);

		if (otherPlayers.Count == Network.connections.Length && Network.isServer)
		{
			NetworkManager.SendUsersNetworkViewID();
			Run.After(0.3f, () => {
				GameStart();
			});
		}
	}

	public void SetTurnOrder(List<NetworkViewID> playerOrder)
	{
		BattleUIManager.Get().SetPlayers(playerOrder, playerClasses);
	}

	public void CreateCharacters()
	{
		myCharacterManager = CharacterManager.CreateInStart(characterPrefab, arrowPrefab, playerClasses[NetworkManager.networkInstance.Id]);
		myCharacterManager.Init();

		Slinqable.Slinq(otherPlayers).ForEach(
			(otherPlayerId) => {
				otherCharacterManagers.Add(otherPlayerId,
						CharacterManager.CreateInStart(characterPrefab, arrowPrefab, playerClasses[otherPlayerId]));
				otherCharacterManagers[otherPlayerId].Init();
			}
		);
	}

	public void GameStart()
	{
        gameOverObj = gameOverImg;

        var enemyInfos = enemyInfoList.GetEnemyInfoList();
		Slinqable.Slinq(enemyInfos).ForEach(
			(enemyInfo) => {
				NetworkManager.MakeEnemy(enemyInfo);
				}
			);

		NetworkManager.SetTurnOrder(
				NetworkManager.networkInstance.Id, otherPlayers);

		CreateCharacters();

		NetworkManager.SendGameStartMessage();
		NetworkManager.SendTurnStartMessage(NetworkManager.networkInstance.Id);
	}
    
	void Awake ()
	{
		gameManagerInstance = this;
		enemyInfoList = new EnemyInfoHolder();
	}

	// Use this for initialization
	void Start ()
	{
		if (Network.isClient)
		{
			NetworkManager.SendUsersNetworkViewID();
		}
	}

	public void InstantiateEnemyByNetwork(string enemyId, int tileKey, Enemy.EnemyType type)
	{
		Tile startTile = TileManager.GetExistTile(tileKey);

		Camera.main.transform.position = new Vector3(
			startTile.transform.position.x,
			startTile.transform.position.y,
			Camera.main.transform.position.z);

		var summonEffect = EffectManager.Get().ShowEnemySpawnEffect(startTile.transform.position);

		var summon = Run.WaitSeconds(0.3f)
			.ExecuteWhenDone(() => {
				EnemyManager enemyManager = EnemyManager.Create(enemyPrefab, startTile, type, enemyId);
				enemyManager.Init();
				enemies.Add(enemyId, enemyManager);
			});

		Run.Join(new List<Run> { summonEffect, summon })
			.ExecuteWhenDone(() => {
				if (Network.isServer)
				{
					Slinqable.Slinq(enemies.Values)
						.FirstOrNone((enemyManager) => enemyManager.GetMoveState() == EnemyManager.MoveState.MakingEnemy)
						.ForEach((enemyManager) => enemyManager.OnEnemyMakingEffectEnd());
				}
			});
	}

	public void MoveEnemy(int tileKey, string enemyId)
	{
		Debug.Log("Move enemy " + enemyId + ", to " + tileKey);
		Run.Coroutine(enemies[enemyId].Move(tileKey));
	}

	public Option<string> GetFirstEnemyId()
	{
		return Slinqable.Slinq(enemies.Keys).FirstOrNone();
	}

	public Option<EnemyManager> GetEnemy(string id)
	{
		return enemies.TryGet(id);
	}

	public void KillEnemy(string enemyId)
	{
		var enemy = enemies[enemyId];
		enemies.Remove(enemyId);
		enemy.Kill();
	}
}
