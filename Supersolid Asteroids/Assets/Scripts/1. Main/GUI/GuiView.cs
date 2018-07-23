using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GuiView : MonoBehaviour {

	//============================================================
	// Inspector Variables:
	//============================================================

	[Header("Base")]

	[SerializeField] private CanvasGroup canvasGroup;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnValidate() {

		// hooks up the inspector variable with the attached component in the editor, and sets up its default values
		canvasGroup = GetComponent<CanvasGroup>();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void HideView() {
		canvasGroup.alpha = 0;
	}

	public void ShowView() {
		canvasGroup.alpha = 1;
	}

}
