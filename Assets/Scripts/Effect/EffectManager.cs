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

	public Run ShowJailEffect(Vector3 position)
	{
		var effect = Instantiate(effectJailPrefab, position, Quaternion.identity);
		return Run.WaitWhile(() => effect != null);
	}

	public Run ShowItemAcquisitionEffect(Vector3 position)
	{
		var effect = Instantiate(itemAcquisitionEffectPrefab, position, Quaternion.identity) as GameObject;
		effect.transform.parent = BattleUIManager.Get().transform;
		effect.transform.localPosition = Vector3.zero;

		return Run.WaitWhile(() => effect != null);
	}
}
