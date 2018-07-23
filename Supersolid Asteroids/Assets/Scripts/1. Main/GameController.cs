using UnityEngine;

public class GameController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int STARTING_ASTEROIDS = 4;

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

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		AsteroidController.AllAsteroidsDestroyedEvent += AsteroidController_AllAsteroidsDestroyed;
		PlayerController.NoLivesRemaining += PlayerController_NoLivesRemaining;
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

		// after a short delay, begin the games next wave
		helper.InvokeActionDelayed(
			() => { StartContinueGame(wavesSurvived);}
			, WAVE_END_DELAY);
	}

	private void PlayerController_NoLivesRemaining() {

		// tell the world that the game has ended
		if (GameOverEvent != null) {
			GameOverEvent.Invoke();
		}
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartGame() {

		// reset our waves survived tracker
		wavesSurvived = 0;

		// let others know that this is a new game so they too can reset
		if (NewGameEvent != null) {
			NewGameEvent.Invoke();
		}

		// spawn the first bunch of asteroids
		StartContinueGame();
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void StartContinueGame(int additionalAsteroids = 0) {
		asteroidController.SpawnAsteroids(STARTING_ASTEROIDS + additionalAsteroids);
	}

}
