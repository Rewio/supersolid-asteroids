using UnityEngine;

public class GameController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int STARTING_ASTEROIDS = 4;
	private const int NUM_WAVES_TO_INCREMENT_SPLIT = 3;
	private const int DEFAULT_ASTEROID_SPLIT_AMOUNT = 2;

	private const float WAVE_END_DELAY = 3f;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler NewGameEvent;
	public static event Helper.EventHandler GameOverEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private Helper helper;
	[SerializeField] private AsteroidController asteroidController;

	//============================================================
	// Private Fields:
	//============================================================

	private int wavesSurvived;

	private bool isPlayingGame;
	private bool isGamePaused;

	private int asteroidSplitAmount = 2;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		AsteroidController.AllAsteroidsDestroyedEvent += AsteroidController_AllAsteroidsDestroyed;
		PlayerController.NoLivesRemaining += PlayerController_NoLivesRemaining;
	}

	private void Update() {

		// if we're not playing the game, we don't need access to these controls
		if (!isPlayingGame) return;

		if (Input.GetKeyDown(KeyCode.P) && !isGamePaused) {
			isGamePaused = true;
			Time.timeScale = 0;
		} else if (Input.GetKeyDown(KeyCode.P) && isGamePaused) {
			isGamePaused = false;
			Time.timeScale = 1;
		}
	}

	private void OnDisable() {
		AsteroidController.AllAsteroidsDestroyedEvent -= AsteroidController_AllAsteroidsDestroyed;
		PlayerController.NoLivesRemaining -= PlayerController_NoLivesRemaining;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void AsteroidController_AllAsteroidsDestroyed() {

		wavesSurvived = wavesSurvived + 1;

		// every x waves, increase the amount of asteroids that spawn when one dies
		if (wavesSurvived % NUM_WAVES_TO_INCREMENT_SPLIT == 0) {
			asteroidSplitAmount = asteroidSplitAmount + 1;
		}

		// after a short delay, begin the games next wave
		helper.InvokeActionDelayed(
			() => { StartContinueGame(asteroidSplitAmount, wavesSurvived); }
			, WAVE_END_DELAY);
	}

	private void PlayerController_NoLivesRemaining() {

		isPlayingGame = false;

		// if somehow the game ends and everything is paused, unpause and resume normal time scale
		if (isGamePaused) {
			isGamePaused = false;
			Time.timeScale = 1;
		}

		// tell the world that the game has ended
		if (GameOverEvent != null) {
			GameOverEvent.Invoke();
		}
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartGame() {

		isPlayingGame = true;

		// reset our asteroid split amount
		asteroidSplitAmount = DEFAULT_ASTEROID_SPLIT_AMOUNT;

		// reset our waves survived tracker
		wavesSurvived = 0;

		// let others know that this is a new game so they too can reset
		if (NewGameEvent != null) {
			NewGameEvent.Invoke();
		}

		// spawn the first bunch of asteroids
		StartContinueGame(asteroidSplitAmount);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void StartContinueGame(int newAsteroidSplitAmount, int additionalAsteroids = 0) {
		asteroidController.SpawnAsteroids(newAsteroidSplitAmount, STARTING_ASTEROIDS + additionalAsteroids);
	}

}
