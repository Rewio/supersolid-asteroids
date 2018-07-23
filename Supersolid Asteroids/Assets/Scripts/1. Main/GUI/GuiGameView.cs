using UnityEngine;

public class GuiGameView : GuiView {

	//============================================================
	// Type Definitions:
	//============================================================

	private enum States {
		Start,
		GameStart,
		GameOver,
		PlayerName,
		End
	}

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Helper helper;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GuiView scoreView;
	[SerializeField] private GuiView livesView;
	[SerializeField] private GuiView gameOverView;
	[SerializeField] private GuiView enterNameView;
	[SerializeField] private GuiView playerNameView;
	[SerializeField] private GuiView acceptNameView;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private float gameOverShowDuration;

	//============================================================
	// Private Fields:
	//============================================================

	private States currentState = States.Start;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void Start() {
		ChangeToNextState();
	}

	private void OnDisable() {
		GameController.GameOverEvent -= GameController_GameOver;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void GameController_GameOver() {
		ChangeToNextState();
	}

	//============================================================
	// State Methods:
	//============================================================

	private void ChangeState(States newState) {

		// if we are not entering a new state, don't do anything
		if (currentState == newState) return;

		// execute a different method based on the state that we are entering
		switch (newState) {
			case States.GameStart:
				State_GameStart();
				break;
			case States.GameOver:
				State_GameOver();
				break;
			case States.End:
				State_End();
				break;
		}

		// record that our state has changed
		currentState = newState;
	}

	private void ChangeToNextState() {
		ChangeState(currentState + 1);
	}

	private void State_GameStart() {

		// ensure that the score and lives views are visible
		scoreView.ShowView();
		livesView.ShowView();

		// listen out for when the game ends, so we can proceed
		GameController.GameOverEvent += GameController_GameOver;
	}

	private void State_GameOver() {

		// show the game over text
		gameOverView.ShowView();

		// after a short delay, proceed with the game
		helper.InvokeActionDelayed(ChangeToNextState, gameOverShowDuration);
	}

	private void State_End() {

		// 
		gameOverView.HideView();
	}
}
