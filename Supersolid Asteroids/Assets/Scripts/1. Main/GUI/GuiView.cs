using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GuiView : MonoBehaviour {

	//============================================================
	// Type Definitions:
	//============================================================

	public enum VisibilityStates {
		Visible, 
		Hidden
	}

	//============================================================
	// Inspector Variables:
	//============================================================

	[Header("Base")]

	[SerializeField] private CanvasGroup canvasGroup;

	//============================================================
	// Public Properties:
	//============================================================

	public VisibilityStates State { get; private set; }

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnValidate() {

		// hooks up the inspector variable with the attached component in the editor, and sets up its default values
		canvasGroup = GetComponent<CanvasGroup>();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void HideView() {
		State = VisibilityStates.Hidden;
		canvasGroup.alpha = 0;
	}

	public void ShowView() {
		State = VisibilityStates.Visible;
		canvasGroup.alpha = 1;
	}

}
