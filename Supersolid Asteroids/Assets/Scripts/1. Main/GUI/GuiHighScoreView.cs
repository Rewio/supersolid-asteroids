using System.Collections.Generic;
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
	[SerializeField] private GuiView highScoreTitleView;
	[SerializeField] private GuiView generatedViewsContainer;
	[SerializeField] private GuiHighScoreEntry highScoresViewPrefab;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Button newGameButton;

	//============================================================
	// Private Fields:
	//============================================================

	private readonly List<GuiHighScoreEntry> createdViews = new List<GuiHighScoreEntry>();

	private List<Score> scores;
	private bool scoreboardViewsCreated;

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
		highScoreTitleView.gameObject.SetActive(enableElseDisable);
		generatedViewsContainer.gameObject.SetActive(enableElseDisable);

		// we only want to do this when the view is being enabled
		if (!enableElseDisable) return;

		// get an updated list of scores
		scores = PlayerData.scoreboard.scores;

		// make sure that the views have been created
		if (!scoreboardViewsCreated) {
			CreateScoreboardViews();
			scoreboardViewsCreated = true;
		}

		// update the information within the views
		UpdateViewInformation();
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void CreateScoreboardViews() {
		for (int i = 0; i < scores.Count; i++) {

			// instantiate a new view
			GuiHighScoreEntry newView = Instantiate(highScoresViewPrefab, generatedViewsContainer.transform);

			// add the desired offset
			newView.SetRectOffset(i);

			// keep track of this view for destruction later on
			createdViews.Add(newView);
		}
	}

	private void UpdateViewInformation() {
		for (int i = 0; i < scores.Count; i++) {

			// grab a view to modify
			GuiHighScoreEntry view = createdViews[i];

			// update the view with the most recent information
			view.SetPlayerName(scores[i].PlayerName);
			view.SetPlayerScore(scores[i].PlayerScore);
		}
	}

}
