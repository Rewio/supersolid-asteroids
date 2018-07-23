using UnityEngine;
using UnityEngine.UI;

public class GuiLivesView : GuiView {

	//============================================================
	// Constants:
	//============================================================

	private const float VIEW_RECT_HEIGHT = 100f;
	private const int NUM_STARTING_LIVES = 3;

	private const float LIFE_REDUCTION_COOLDOWN = 2f;

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

	private float nextUpdateAllowed;

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

		// if enough time hasn't passed yet, just back out
		if (Time.time < nextUpdateAllowed) return;

		// prevent further life reduction for the time being
		// edge case, the player can collide with 2 asteroids at once, resulting in the graphic updating twice but only losing one life.
		nextUpdateAllowed = Time.time + LIFE_REDUCTION_COOLDOWN;

		remainingLives = remainingLives - 1;
		UpdateLivesView(remainingLives);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void UpdateLivesView(int livesLeft) {
		viewRectDimensions.x = widthPerLife * livesLeft;
		livesPanel.rectTransform.sizeDelta = viewRectDimensions;
	}

}
