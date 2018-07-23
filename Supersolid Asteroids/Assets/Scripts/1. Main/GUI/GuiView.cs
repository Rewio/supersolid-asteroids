using UnityEngine;

public abstract class GuiView : MonoBehaviour {

	//============================================================
	// Inspector Variables:
	//============================================================

	[Header("Base")]

	[SerializeField] private CanvasGroup canvasGroup;

	//============================================================
	// Public Methods:
	//============================================================

	public virtual void HideView() {
		canvasGroup.alpha = 0;
	}

	public virtual void ShowView() {
		canvasGroup.alpha = 1;
	}

}
