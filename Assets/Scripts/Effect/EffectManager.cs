using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
	private static EffectManager instance;
	public static EffectManager Get()
	{
		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	public GameObject effectJailPrefab;
	public GameObject itemAcquisitionEffectPrefab;
	public GameObject enemySpawnEffectPrefab;

	public Run ShowJailEffect(Vector3 position)
	{
		var effect = Instantiate(effectJailPrefab, position, Quaternion.identity);
		return Run.WaitWhile(() => effect != null);
	}

	public Run ShowItemAcquisitionEffect(Vector3 position, Character.Item item)
	{
		var effect = Instantiate(itemAcquisitionEffectPrefab, position, Quaternion.identity) as GameObject;
		effect.transform.parent = BattleUIManager.Get().transform;
		effect.transform.localPosition = Vector3.zero;

		var itemSprite = BattleUIManager.Get().GetSpriteOfItem(item);
		effect.GetComponentInChildren<SpriteRenderer>().sprite = itemSprite;

		return Run.WaitWhile(() => effect != null);
	}

	public Run ShowEnemySpawnEffect(Vector3 position)
	{
		var effect = Instantiate(enemySpawnEffectPrefab, position, Quaternion.identity);
		return Run.WaitWhile(() => effect != null);
	}
}
