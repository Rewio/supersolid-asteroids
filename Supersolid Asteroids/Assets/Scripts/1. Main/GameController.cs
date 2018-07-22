using UnityEngine;

public class GameController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int STARTING_ASTEROIDS = 4;

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private AsteroidController asteroidController;
	[SerializeField] private PlayerController playerController;

	//============================================================
	// Private Fields:
	//============================================================

	private int wavesSurvived;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		AsteroidController.AllAsteroidsDestroyedEvent += AsteroidController_AllAsteroidsDestroyed;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			StartContinueGame();
		}
	}

	private void OnDisable() {
		AsteroidController.AllAsteroidsDestroyedEvent -= AsteroidController_AllAsteroidsDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void AsteroidController_AllAsteroidsDestroyed() {
		wavesSurvived++;
		StartContinueGame(wavesSurvived);
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartContinueGame(int additionalAsteroids = 0) {
		asteroidController.SpawnAsteroids(STARTING_ASTEROIDS + additionalAsteroids);
	}

}
