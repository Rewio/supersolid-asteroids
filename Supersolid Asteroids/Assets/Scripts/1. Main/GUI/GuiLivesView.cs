using UnityEngine;
using UnityEngine.UI;

public class GuiLivesView : GuiView {

	//============================================================
	// Constants:
	//============================================================

	private const float VIEW_RECT_HEIGHT = 100f;
	private const int NUM_STARTING_LIVES = 3;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Image livesPanel;
	[SerializeField] private float widthPerLife;

	//============================================================
	// Private Fields:
	//============================================================

	private Vector2 viewRectDimensions;
	private int playersRemainingLives;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		PlayerController.RemainingLivesUpdate += PlayerController_RemainingLivesUpdate;
		GameController.NewGameEvent += GameController_NewGame;
	}

	private void Start() {
		playersRemainingLives = NUM_STARTING_LIVES;
		viewRectDimensions = new Vector2(widthPerLife * NUM_STARTING_LIVES, VIEW_RECT_HEIGHT);
	}

	private void OnDisable() {
		PlayerController.RemainingLivesUpdate -= PlayerController_RemainingLivesUpdate;
		GameController.NewGameEvent -= GameController_NewGame;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void PlayerController_RemainingLivesUpdate(int remaininglives) {
		playersRemainingLives = remaininglives;
		UpdateLivesView(playersRemainingLives);
	}

	private void GameController_NewGame() {

		// reset our local lives tracker, and the graphic showing remaining lives
		playersRemainingLives = NUM_STARTING_LIVES;
		UpdateLivesView(playersRemainingLives);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void UpdateLivesView(int livesLeft) {
		viewRectDimensions.x = widthPerLife * livesLeft;
		livesPanel.rectTransform.sizeDelta = viewRectDimensions;
	}

}
