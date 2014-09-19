using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	void OnEnable() {
		Destroy(gameObject);
	}
}
