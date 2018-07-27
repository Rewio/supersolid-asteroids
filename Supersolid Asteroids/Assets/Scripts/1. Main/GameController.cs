using UnityEngine;

public class GameController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int STARTING_ASTEROIDS = 4;

	private const int DEFAULT_ASTEROID_SPLIT_AMOUNT = 2;
	private const int NUM_WAVES_TO_INCREMENT_SPLIT  = 3;

	private const int DEFAULT_ENEMY_SPAWN_AMOUNT   = 0;
	private const int NUM_WAVES_TO_INCREMENT_SPAWN = 2;

	private const float WAVE_END_DELAY = 3f;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler NewGame;
	public static event Helper.EventHandler GameOver;
	public static event Helper.EventHandler GamePaused;

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private Helper helper;
	[SerializeField] private AsteroidController asteroidController;
	[SerializeField] private EnemyController enemyController;

	//============================================================
	// Private Fields:
	//============================================================

	private int wavesSurvived;

	private bool isPlayingGame;
	private bool isGamePaused;

	private int asteroidSplitAmount = 2;
	private int enemySpawnAmount;

	private bool allAsteroidsDestroyed;
	private bool allEnemiesDestroyed;

	private bool hasLostFocus;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnEnable() {
		AsteroidController.AllAsteroidsDestroyed += AsteroidController_AllAsteroidsDestroyed;
		PlayerController.NoLivesRemaining += PlayerController_NoLivesRemaining;
		EnemyController.AllEnemiesDestroyed += EnemyController_AllEnemiesDestroyed;
	}

	protected void Update() {

		// if we're not playing the game, we don't need access to these controls
		if (!isPlayingGame) return;

		// allows for pausing the game, also force pauses if the application loses focus
		if (Input.GetKeyDown(KeyCode.P) || !Application.isFocused && !hasLostFocus) {
			PauseGame();
		}
	}

	protected void OnDisable() {
		AsteroidController.AllAsteroidsDestroyed -= AsteroidController_AllAsteroidsDestroyed;
		PlayerController.NoLivesRemaining -= PlayerController_NoLivesRemaining;
		EnemyController.AllEnemiesDestroyed -= EnemyController_AllEnemiesDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void AsteroidController_AllAsteroidsDestroyed() {

		// flag that all asteroids have been destroyed, used to check if the wave has ended
		allAsteroidsDestroyed = true;

		// responsible for checking if asteroids and enemies are all dead and starting the next wave
		HasWaveEndedCheck();
	}

	private void PlayerController_NoLivesRemaining() {

		isPlayingGame = false;

		// if somehow the game ends and everything is paused, unpause and resume normal time scale
		if (isGamePaused) {
			isGamePaused = false;
			Time.timeScale = 1;
		}

		// tell the world that the game has ended
		if (GameOver != null) {
			GameOver.Invoke();
		}
	}

	private void EnemyController_AllEnemiesDestroyed() {

		// flag that all enemies have been destroyed, used to check if the wave has ended
		allEnemiesDestroyed = true;

		// responsible for checking if asteroids and enemies are all dead and starting the next wave
		HasWaveEndedCheck();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartGame() {

		isPlayingGame = true;

		// reset our asteroid split amount
		asteroidSplitAmount = DEFAULT_ASTEROID_SPLIT_AMOUNT;

		// reset our enemy spawn amount
		enemySpawnAmount = DEFAULT_ENEMY_SPAWN_AMOUNT;

		// reset our waves survived tracker
		wavesSurvived = 0;

		// reset the flags that determine the end of the wave
		allAsteroidsDestroyed = false;
		allEnemiesDestroyed   = false;

		// let others know that this is a new game so they too can reset
		if (NewGame != null) {
			NewGame.Invoke();
		}

		// spawn the first bunch of asteroids
		StartContinueGame();
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void HasWaveEndedCheck() {

		// if there are either asteroids or enemies alive, the wave has not been cleared
		if (!allAsteroidsDestroyed || !allEnemiesDestroyed) return;

		// increment the number of waves we have survived
		wavesSurvived = wavesSurvived + 1;

		// every x waves, increase the amount of asteroids that spawn when one dies
		if (wavesSurvived % NUM_WAVES_TO_INCREMENT_SPLIT == 0) {
			asteroidSplitAmount = asteroidSplitAmount + 1;
		}

		// again, every x waves, increase the number of enemy ships that will spawn in the wave
		if (wavesSurvived % NUM_WAVES_TO_INCREMENT_SPAWN == 0) {
			enemySpawnAmount = enemySpawnAmount + 1;
		}

		// after a short delay, begin the games next wave
		helper.InvokeActionDelayed(
			() => { StartContinueGame(wavesSurvived, asteroidSplitAmount, enemySpawnAmount); }
			, WAVE_END_DELAY);
	}

	private void StartContinueGame(int additionalAsteroids = 0, int newAsteroidSplitAmount = 0, int numEnemiesToSpawn = 0) {
		asteroidController.SpawnAsteroids(STARTING_ASTEROIDS + additionalAsteroids, newAsteroidSplitAmount);
		allAsteroidsDestroyed = false;

		// if there are no enemies to spawn this wave, flag they are all destroyed, then back out
		if (numEnemiesToSpawn == 0) {
			allEnemiesDestroyed = true;
			return;
		}

		// otherwise, tell the controller how many enemies need to be spawned this wave and flag that they are to be killed
		enemyController.SetupEnemySpawning(numEnemiesToSpawn);
		allEnemiesDestroyed = false;
	}

	private void PauseGame() {

		// flag that we have lost focus, even if we haven't, otherwise it gets called constantly in update.
		hasLostFocus = !hasLostFocus;

		if (isGamePaused) {
			isGamePaused = false;
			Time.timeScale = 1;
		}
		else {
			isGamePaused = true;
			Time.timeScale = 0;
		}

		if (GamePaused == null) return;
		GamePaused.Invoke();
	}

}
