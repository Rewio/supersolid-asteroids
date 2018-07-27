using UnityEngine;
using UnityEngine.UI;

public class GuiScoreView : GuiView {

	//============================================================
	// Constants:
	//============================================================

	private const int SCORE_LARGE_ASTEROID  = 20;
	private const int SCORE_MEDIUM_ASTEROID = 50;
	private const int SCORE_SMALL_ASTEROID = 100;
	private const int SCORE_ENEMY_SHIP     = 200;

	private const int SCORE_NEW_LIFE_INCREMENT = 10000;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler NewLifeEarned;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Text scoreText;

	//============================================================
	// Private Fields:
	//============================================================

	private int currentScore;

	private int scoreWhenNewLifeGranted = SCORE_NEW_LIFE_INCREMENT;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnEnable() {
		Asteroid.AsteroidDestroyedEvent += Asteroid_AsteroidDestroyed;
		GameController.NewGame     += GameController_NewGame;
		GameController.GameOver    += GameController_GameOver;
		Enemy.EnemyDestroyed += Enemy_EnemyDestroyed;
	}

	protected void Start() {
		currentScore = int.Parse(scoreText.text);
	}

	protected void OnDisable() {
		Asteroid.AsteroidDestroyedEvent -= Asteroid_AsteroidDestroyed;
		GameController.NewGame     -= GameController_NewGame;
		GameController.GameOver    -= GameController_GameOver;
		Enemy.EnemyDestroyed -= Enemy_EnemyDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Asteroid_AsteroidDestroyed(Asteroid asteroid, Vector2 asteroidDeathPosition) {

		// increase the score based on which size asteroid was destroyed
		switch (asteroid.AsteroidSize) {
			case AsteroidController.AsteroidSizes.Large:
				IncreaseScore(SCORE_LARGE_ASTEROID);
				break;
			case AsteroidController.AsteroidSizes.Medium:
				IncreaseScore(SCORE_MEDIUM_ASTEROID);
				break;
			case AsteroidController.AsteroidSizes.Small:
				IncreaseScore(SCORE_SMALL_ASTEROID);
				break;
		}
	}

	private void Enemy_EnemyDestroyed() {
		IncreaseScore(SCORE_ENEMY_SHIP);
	}

	private void GameController_NewGame() {

		// reset our new life score
		scoreWhenNewLifeGranted = SCORE_NEW_LIFE_INCREMENT;

		// reset our score value, and the ui's score value
		currentScore = 0;
		scoreText.text = currentScore.ToString();
	}

	private void GameController_GameOver() {

		// if the game ends, update the player data object with the players score
		PlayerData.PlayerScore = currentScore;
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void IncreaseScore(int additionalScore) {
		currentScore += additionalScore;
		scoreText.text = currentScore.ToString();

		if (currentScore < scoreWhenNewLifeGranted) return;

		// increment the score required to earn a new life
		scoreWhenNewLifeGranted = scoreWhenNewLifeGranted + SCORE_NEW_LIFE_INCREMENT;

		// tell those that are listening that a new life has been earned
		if (NewLifeEarned != null) {
			NewLifeEarned.Invoke();
		}
	}

}
