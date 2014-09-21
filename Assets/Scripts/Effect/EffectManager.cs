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

	public Run ShowJailEffect(Vector3 position)
	{
		var effect = Instantiate(effectJailPrefab, position, Quaternion.identity);
		return Run.WaitWhile(() => effect != null);
	}
}
