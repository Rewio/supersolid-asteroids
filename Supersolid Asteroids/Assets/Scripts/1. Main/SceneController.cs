using UnityEngine;

public class SceneController : MonoBehaviour {

	//============================================================
	// Type Definitions:
	//============================================================

	private enum States {
		PreStart,
		Start,
		Menu,
		Game, 
		HighScores,
		End
	}

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GuiController guiController;
	[SerializeField] private GameController gameController;

	//============================================================
	// Private Fields:
	//============================================================

	private States currentState = States.PreStart;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		GuiMenuView.PlayGameEvent     += GuiMenuView_PlayGame;
		GuiMenuView.HighScoresEvent   += GuiMenuView_HighScores;
		GuiGameView.GameFinishedEvent += GuiGameView_GameFinished;
		GuiHighScoreView.NewGameEvent += GuiHighScoreView_NewGame;
	}

	private void Start() {

		// setup the file directory and save file
		FileUtil.Setup();

		// load the scoreboard from disk
		PlayerData.scoreboard = FileUtil.LoadScoreboard();

		// if the scoreboard is null, there is no save data, so create a scoreboard and save it
		if (PlayerData.scoreboard == null) {
			PlayerData.scoreboard = new Scoreboard();
			FileUtil.SaveScoreboard(PlayerData.scoreboard);
		}

		ChangeToNextState();
	}

	private void OnDisable() {
		GuiMenuView.PlayGameEvent     -= GuiMenuView_PlayGame;
		GuiMenuView.HighScoresEvent   -= GuiMenuView_HighScores;
		GuiGameView.GameFinishedEvent -= GuiGameView_GameFinished;
		GuiHighScoreView.NewGameEvent -= GuiHighScoreView_NewGame;
	}

	//============================================================
	// State Methods:
	//============================================================

	private void ChangeState(States newState) {

		// if we are not entering a new state, don't do anything
		if (currentState == newState) return;

		// record that our state has changed before executing any state related logic
		currentState = newState;

		// execute a different method based on the state that we are entering
		switch (newState) {
			case States.Start:
				State_Start();
				break;
			case States.Menu:
				State_MenuStart();
				break;
			case States.Game:
				State_Game();
				break;
			case States.HighScores:
				State_HighScoresEnter();
				break;
			case States.End:
				State_End();
				break;
		}
	}

	private void ChangeToNextState() {
		ChangeState(currentState + 1);
	}

	private void State_Start() {

		// have all views hidden by default.
		guiController.HideAllViews();

		ChangeToNextState();
	}

	private void State_MenuStart() {

		// present the menu view
		guiController.ShowHideMenuView(true);
	}

	private void State_MenuExit() {

		// hide the menu view
		guiController.ShowHideMenuView(false);
	}

	private void State_Game() {

		// hide the cursor during the game, it's hideous
		Cursor.visible = false;

		// show the game view, and initialise the states
		guiController.ShowHideGameView(true);
		
		// start the game for the player and game controllers
		gameController.StartGame();
	}

	private void State_HighScoresEnter() {

		// make sure the other views are hidden
		guiController.ShowHideMenuView(false);
		guiController.ShowHideGameView(false);

		// present the high-scores view
		guiController.ShowHideHighScoreView(true);
	}

	private void State_HighScoresExit() {
		guiController.ShowHideHighScoreView(false);
	}

	private void State_End() {

	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void GuiMenuView_PlayGame() {
		State_MenuExit();
		ChangeState(States.Game);
	}

	private void GuiMenuView_HighScores() {
		State_MenuExit();
		ChangeState(States.HighScores);
	}

	private void GuiGameView_GameFinished() {

		// as the game has finished, record the players score, add it to the scoreboard, then save that scoreboard to disk
		Score newScore = new Score(PlayerData.PlayerName, PlayerData.PlayerScore);
		PlayerData.scoreboard.AddNewScore(newScore);
		FileUtil.SaveScoreboard(PlayerData.scoreboard);

		ChangeState(States.HighScores);
	}

	private void GuiHighScoreView_NewGame() {
		State_HighScoresExit();
		ChangeState(States.Menu);
	}

}
