using UnityEngine;
using System.Collections;

public class RightScroller : MonoBehaviour
{
	public static float openedXPos = -11;
	public static float closedXPos = -1;

	enum State
	{
		Opening,
		Closing
	}

	State state = State.Opening;
	Run waitingAnimation;

	bool CheckDraggedEnoughToOpen()
	{
		return transform.localPosition.x < closedXPos - 1;
	}

	bool CheckDraggedEnoughToClose()
	{
		return transform.localPosition.x > openedXPos + 1;
	}

	void Open()
	{
		audio.Play ();
		Vector3 toMoveLocalPos = new Vector3(openedXPos, transform.localPosition.y, transform.localPosition.z);
		Vector3 toMoveWorldPos = transform.parent.TransformPoint(toMoveLocalPos);
		waitingAnimation = Run.WaitSeconds(0.5f);
		iTween.MoveTo(gameObject, iTween.Hash("position", toMoveWorldPos, "easetype", iTween.EaseType.easeOutCubic, "time", 0.5f));
	}

	void Close()
	{
		audio.Play ();
		Vector3 toMoveLocalPos = new Vector3(closedXPos, transform.localPosition.y, transform.localPosition.z);
		Vector3 toMoveWorldPos = transform.parent.TransformPoint(toMoveLocalPos);
		waitingAnimation = Run.WaitSeconds(0.5f);
		iTween.MoveTo(gameObject, iTween.Hash("position", toMoveWorldPos, "easetype", iTween.EaseType.easeOutCubic, "time", 0.5f));
	}

	void SetInitialState()
	{
		var diffFromClosed = Mathf.Abs(transform.localPosition.x - closedXPos);
		var diffFromOpened = Mathf.Abs(transform.localPosition.x - openedXPos);

		if (diffFromClosed > diffFromOpened)
		{
			state = State.Closing;
		}
		else
		{
			state = State.Opening;
		}
	}

	void OnMouseUpAsButton()
	{
		if (waitingAnimation != null && !waitingAnimation.isDone)
		{
			return;
		}
		SetInitialState();

		if (state == State.Closing)
		{
				Close();
		}
		else
		{
				Open();
		}
	}

	IEnumerator OnMouseDrag()
	{
		if (waitingAnimation != null && !waitingAnimation.isDone)
		{
			yield break;
		}

		Vector3 startPoint = transform.position;
		SetInitialState();

		Vector3 currentPosition = transform.position;
		Vector3 screenPoint = BattleUIManager.Get().uiCamera.WorldToScreenPoint(transform.position);
		Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		yield return null;

		while(Input.GetMouseButton(0))
		{
			if (state == State.Closing)
			{
				if (CheckDraggedEnoughToClose())
				{
					Close();
					yield break;
				}
			}
			else
			{
				if (CheckDraggedEnoughToOpen())
				{
					Open();
					yield break;
				}
			}

			Vector3 curMousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 diffScreenPoint = curMousePoint - mousePoint;

			Vector3 newScreenPoint = screenPoint + diffScreenPoint;
			Vector3 newPosition = BattleUIManager.Get().uiCamera.ScreenToWorldPoint(newScreenPoint);

			transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);
			transform.localPosition = new Vector3(
					Mathf.Clamp(transform.localPosition.x, openedXPos, closedXPos),
					transform.localPosition.y,
					transform.localPosition.z);

			screenPoint = BattleUIManager.Get().uiCamera.WorldToScreenPoint(transform.position);
			mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			yield return null;
		}

		if (state == State.Closing)
		{
			if (CheckDraggedEnoughToClose())
			{
				Close();
				yield break;
			}
		}
		else
		{
			if (CheckDraggedEnoughToOpen())
			{
				Open();
				yield break;
			}
		}
	}
}
