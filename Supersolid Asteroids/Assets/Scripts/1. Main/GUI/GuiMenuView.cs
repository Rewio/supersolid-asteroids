using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuiMenuView : GuiView {

	//============================================================
	// Type Definitions:
	//============================================================

	private enum Events {
		PlayGame,
		HighScores
	}

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler PlayGameEvent;
	public static event Helper.EventHandler HighScoresEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private GuiView asteroidsTitleView;
	[SerializeField] private GuiView playGameView;
	[SerializeField] private GuiView highScoresView;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Button playGameButton;
	[SerializeField] private Button highScoresButton;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		playGameButton.onClick.AddListener(ButtonsOnClick(Events.PlayGame));
		highScoresButton.onClick.AddListener(ButtonsOnClick(Events.HighScores));
	}

	private void OnDisable() {
		playGameButton.onClick.RemoveAllListeners();
		highScoresButton.onClick.RemoveAllListeners();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void EnableDisableViews(bool enableElseDisable) {
		asteroidsTitleView.gameObject.SetActive(enableElseDisable);
		playGameView.gameObject.SetActive(enableElseDisable);
		highScoresView.gameObject.SetActive(enableElseDisable);
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private static UnityAction ButtonsOnClick(Events eventToRaise) {

		// empty action to be returned later
		UnityAction action = null;

		// if the play button was clicked, return an action invoking the playgame event
		if (eventToRaise == Events.PlayGame) {
			if (PlayGameEvent != null) {
				action = () => { PlayGameEvent.Invoke(); };
			}
		}

		// otherwise, return an action invoking the highscores event
		else {
			if (HighScoresEvent != null) {
				action = () => { HighScoresEvent.Invoke(); };
			}
		}
		return action;
	}

}
