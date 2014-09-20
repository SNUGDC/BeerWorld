using UnityEngine;
using System.Collections;

public class UIButtonMessage : MonoBehaviour
{
	public System.Action buttonClickEvent;

	public void OnMouseUpAsButton()
	{
		if (buttonClickEvent != null)
		{
			buttonClickEvent();
		}
	}
}
