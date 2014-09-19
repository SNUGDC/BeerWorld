using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindGameObjectByName
{
	public static GameObject FindChild(GameObject parent, string name)
	{
		Queue<Transform> searchQueue = new Queue<Transform>();
		searchQueue.Enqueue(parent.transform);

		while (searchQueue.Count > 0)
		{
			var parentTransform = searchQueue.Dequeue();
			foreach (Transform child in parentTransform)
			{
				if (child.name == name)
				{
					return child.gameObject;
				}
				else
				{
					searchQueue.Enqueue(child);
				}
			}
		}

		return null;
	}
}
