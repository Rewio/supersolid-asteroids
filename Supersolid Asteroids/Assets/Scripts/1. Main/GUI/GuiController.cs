using UnityEngine;

public class GuiController : MonoBehaviour {

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GuiMenuView menuView;
	[SerializeField] private GuiGameView gameView;
	[SerializeField] private GuiView highScoreView;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public void HideAllViews() {
		ShowHideMenuView(false);
		ShowHideGameView(false);
		ShowHideHighScoreView(false);
	}

	public void ShowHideMenuView(bool enableElseDisable) {
		menuView.EnableDisableViews(enableElseDisable);
	}

	public void ShowHideGameView(bool enableElseDisable) {
		gameView.gameObject.SetActive(enableElseDisable);
		if (enableElseDisable) { gameView.StartGameView(); }
	}

	public void ShowHideHighScoreView(bool enableElseDisable) {

	}
}
