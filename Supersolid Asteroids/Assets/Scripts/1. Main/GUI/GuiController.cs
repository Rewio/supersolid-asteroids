using System;
using UnityEngine;

public class GuiController : MonoBehaviour {

	//============================================================
	// Type Defintions:
	//============================================================

	public enum Views {
		Menu,
		Game,
		HighScore,
		None
	}

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GameObject menuView;
	[SerializeField] private GameObject gameView;
	[SerializeField] private GameObject highScoreView;

	//============================================================
	// Private Fields:
	//============================================================

	private Views currentlyActiveView;

	//============================================================
	// Public Methods:
	//============================================================

	public void ShowView(Views viewToShow) {

		// if we're trying to show the view that's active, back out
		if (viewToShow == currentlyActiveView) return;

		// record that we now have an active view
		currentlyActiveView = viewToShow;

		// hide whichever view is currently active
		HideCurrentView();

		switch (viewToShow) {
			case Views.Menu:
				menuView.SetActive(true);
				break;
			case Views.Game:
				gameView.SetActive(true);
				break;
			case Views.HighScore:
				highScoreView.SetActive(true);
				break;
		}
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void HideCurrentView() {

		// make sure that a view is actually active first
		if (currentlyActiveView == Views.None) return;

		switch (currentlyActiveView) {
			case Views.Menu:
				menuView.SetActive(false);
				break;
			case Views.Game:
				gameView.SetActive(false);
				break;
			case Views.HighScore:
				gameView.SetActive(false);
				break;
		}

		// flag that there are currently no active views
		currentlyActiveView = Views.None;
	}

}
