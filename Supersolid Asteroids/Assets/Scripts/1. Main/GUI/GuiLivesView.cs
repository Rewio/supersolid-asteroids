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
	[SerializeField] private AudioSource audioSource;

	//============================================================
	// Private Fields:
	//============================================================

	private Vector2 viewRectDimensions;
	private int playersRemainingLives;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnEnable() {
		PlayerController.RemainingLivesUpdate += PlayerController_RemainingLivesUpdate;

		GameController.NewGame += GameController_NewGame;
		GuiScoreView.NewLifeEarned  += GuiScoreView_NewLifeEarned;
	}

	protected void Start() {
		playersRemainingLives = NUM_STARTING_LIVES;
		viewRectDimensions = new Vector2(widthPerLife * NUM_STARTING_LIVES, VIEW_RECT_HEIGHT);
	}

	protected void OnDisable() {
		PlayerController.RemainingLivesUpdate -= PlayerController_RemainingLivesUpdate;

		GameController.NewGame -= GameController_NewGame;
		GuiScoreView.NewLifeEarned  -= GuiScoreView_NewLifeEarned;
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

	private void GuiScoreView_NewLifeEarned() {
		playersRemainingLives = playersRemainingLives + 1;
		UpdateLivesView(playersRemainingLives);

		// play the new life sound
		audioSource.Play();
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void UpdateLivesView(int livesLeft) {
		viewRectDimensions.x = widthPerLife * livesLeft;
		livesPanel.rectTransform.sizeDelta = viewRectDimensions;
	}

}
