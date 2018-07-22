using System;
using System.Collections;
using UnityEngine;

public class Helper : MonoBehaviour {

	//============================================================
	// Decorator Variables:
	//============================================================

	public const float INSPECTOR_SPACE = 6f;
	public const float INSPECTOR_SPACE_BIG = 10f;

	//============================================================
	// Event Handlers:
	//============================================================

	public delegate void EventHandler();

	//============================================================
	// Helpful Methods:
	//============================================================

	public void InvokeActionDelayed (Action action, float delay) {
		StartCoroutine(InvokeActionDelayedCoroutine(action, delay));
	}

	private IEnumerator InvokeActionDelayedCoroutine (Action action, float delay) {

		// wait the desired amount of time
		yield return new WaitForSeconds(delay);

		// after waiting, invoke the action
		action.Invoke();
	}

}