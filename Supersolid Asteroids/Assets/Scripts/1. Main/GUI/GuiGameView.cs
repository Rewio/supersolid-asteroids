using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GuiGameView : GuiView {

	//============================================================
	// Constants:
	//============================================================

	private const string INPUT_REGEX_STRING = "[^A-Za-z0-9 ]+";

	private const int MAXIMUM_NAME_LENGTH = 20;

	//============================================================
	// Type Definitions:
	//============================================================

	public enum States {
		PreStart,
		Start,
		GameStart,
		GameOver,
		EnterPlayerName,
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

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Text playerNameTextField;
	[SerializeField] private Button playerNameAcceptButton;

	//============================================================
	// Private Fields:
	//============================================================

	private States currentState = States.PreStart;

	private IEnumerator collectInputEnumerator;

	private string playerNameText = "";

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnDisable() {
		GameController.GameOverEvent -= GameController_GameOver;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void GameController_GameOver() {
		ChangeToNextState();
	}

	private void PlayerNameButtonClicked() {

		// remove all leading and trailing spaces, they are unnecessary
		playerNameText = playerNameText.Trim();

		// if they have yet to enter their name, do not proceed
		if (playerNameText.Length == 0 || playerNameText.Equals("")) return;

		ChangeToNextState();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartGameView() {
		ChangeToNextState();
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
			case States.GameStart:
				State_GameStart();
				break;
			case States.GameOver:
				State_GameOver();
				break;
			case States.EnterPlayerName:
				State_EnterPlayerName();
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

		// ensure that all views are hidden by default, so they can be enabled by their respective states
		HideAllViews();

		ChangeToNextState();
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

	private void State_EnterPlayerName() {

		// hide the game over text view
		gameOverView.HideView();

		// show the views responsible for collecting the players name
		enterNameView.ShowView();
		playerNameView.ShowView();
		acceptNameView.ShowView();

		// start the text capture coroutine so the player can enter their name
		StartCoroutine(collectInputEnumerator = CollectInputCoroutine());

		// add a listener so we know if the button has been clicked
		playerNameAcceptButton.onClick.AddListener(PlayerNameButtonClicked);
	}

	private void State_End() {
		
		// we no-longer care if the button has been clicked, remove the listener
		playerNameAcceptButton.onClick.RemoveAllListeners();

		// we are proceeding into the next part of the game, these views are no-longer needed
		HideAllViews();
	}

	//============================================================
	// Coroutines:
	//============================================================

	private IEnumerator CollectInputCoroutine() {

		// grab a local reference of the players name text field for manipulation
		playerNameText = playerNameTextField.text;

		while (collectInputEnumerator != null) {

			// delete the last character, if there is a character to be deleted
			if (Input.GetKeyDown(KeyCode.Backspace) && playerNameText.Length > 0) {
				playerNameText = playerNameText.Remove(playerNameText.Length - 1);

			// if the return key is pressed, act as if the accept button has been clicked
			} else if (Input.GetKeyDown(KeyCode.Return)) {
				PlayerNameButtonClicked();
			}

			// if we are going to exceed the texts maximum length, dont do anything
			if (playerNameText.Length < MAXIMUM_NAME_LENGTH) {

				// capture any raw keyboard input
				string keyboardInput = Input.inputString;

				// parse it with regex, allowing only letters, numbers and spaces through
				keyboardInput = Regex.Replace(keyboardInput, INPUT_REGEX_STRING, "");

				// append this text to our players name text field
				playerNameText           += keyboardInput;
				playerNameTextField.text  = playerNameText;
			}
			yield return null;
		}
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void HideAllViews() {
		scoreView.HideView();
		livesView.HideView();
		gameOverView.HideView();
		enterNameView.HideView();
		playerNameView.HideView();
		acceptNameView.HideView();
	}

}
