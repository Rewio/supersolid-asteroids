using UnityEngine;
using UnityEngine.UI;

public class GuiHighScoreView : GuiView {

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler NewGameEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private GuiView continueView;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Button newGameButton;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		newGameButton.onClick.AddListener(NewGameButtonClicked);
	}

	private void OnDisable() {
		newGameButton.onClick.RemoveAllListeners();
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void NewGameButtonClicked() {
		if (NewGameEvent != null) {
			NewGameEvent.Invoke();
		}
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void EnableDisableViews(bool enableElseDisable) {
		continueView.gameObject.SetActive(enableElseDisable);
	}

}
