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
	private int remainingLives;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		Player.PlayerDestroyedEvent += Player_PlayerDestroyed;
	}

	private void Start() {
		remainingLives = NUM_STARTING_LIVES;
		viewRectDimensions = new Vector2(widthPerLife * NUM_STARTING_LIVES, VIEW_RECT_HEIGHT);
	}

	private void OnDisable() {
		Player.PlayerDestroyedEvent -= Player_PlayerDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Player_PlayerDestroyed() {
		remainingLives--;
		UpdateLivesView(remainingLives);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void UpdateLivesView(int remainingLives) {
		viewRectDimensions.x = widthPerLife * remainingLives;
		livesPanel.rectTransform.sizeDelta = viewRectDimensions;
	}

}
