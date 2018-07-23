﻿using UnityEngine;

public class GameController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int STARTING_ASTEROIDS = 4;

	private const float WAVE_END_DELAY = 3f;

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

		// after a short delay, begin the games next wave
		helper.InvokeActionDelayed(
			() => { StartContinueGame(wavesSurvived);}
			, WAVE_END_DELAY);
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartContinueGame(int additionalAsteroids = 0) {
		asteroidController.SpawnAsteroids(STARTING_ASTEROIDS + additionalAsteroids);
	}

}
