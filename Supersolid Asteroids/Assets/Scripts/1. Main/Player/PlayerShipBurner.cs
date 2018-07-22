using System.Collections;
using UnityEngine;

public class PlayerShipBurner : MonoBehaviour {

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private SpriteRenderer shipBurnerRenderer;

	//============================================================
	// Private Fields:
	//============================================================

	private bool isPlayerMoving;
	private bool isShipBurnerEnabled;

	private IEnumerator burnerStatusToggleEnumerator;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void Start() {
		StartCoroutine(burnerStatusToggleEnumerator = BurnerStatusToggleCoroutine());
	}

	private void Update() {

		// flag that we are moving so we can start toggling the burner on and off.
		if (Input.GetKey(KeyCode.W)) {
			isPlayerMoving = true;
		}

		// flag we are no-longer moving so we can prevent the burner from being toggled.
		if (Input.GetKeyUp(KeyCode.W)) {
			isPlayerMoving = false;
		}
	}

	private void OnDisable() {
		StopCoroutine(burnerStatusToggleEnumerator);
		burnerStatusToggleEnumerator = null;
	}

	//============================================================
	// Coroutines:
	//============================================================

	private IEnumerator BurnerStatusToggleCoroutine() {
		while (burnerStatusToggleEnumerator != null) {

			// if the player isn't moving, ensure the burner is disabled
			if (!isPlayerMoving) {
				shipBurnerRenderer.enabled = false;
				yield return new WaitForSeconds(0.1f);
			}

			// otherwise, toggle the burners active state for the desired effect
			else {
				isShipBurnerEnabled        = !isShipBurnerEnabled;
				shipBurnerRenderer.enabled = isShipBurnerEnabled;
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}
