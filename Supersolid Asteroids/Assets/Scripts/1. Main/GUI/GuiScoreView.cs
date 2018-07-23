﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class GuiScoreView : GuiView {

	//============================================================
	// Constants:
	//============================================================

	private const int SCORE_LARGE_ASTEROID  = 20;
	private const int SCORE_MEDIUM_ASTEROID = 50;
	private const int SCORE_SMALL_ASTEROID  = 100;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Text scoreText;

	//============================================================
	// Private Fields:
	//============================================================

	private int currentScore;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		Asteroid.AsteroidDestroyedEvent += Asteroid_AsteroidDestroyed;
	}

	private void Start() {
		currentScore = int.Parse(scoreText.text);
	}

	private void OnDisable() {
		Asteroid.AsteroidDestroyedEvent -= Asteroid_AsteroidDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Asteroid_AsteroidDestroyed(Asteroid asteroid, Vector2 asteroiddeathposition) {

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

	//============================================================
	// Private Methods:
	//============================================================

	private void IncreaseScore(int additionalScore) {
		currentScore += additionalScore;
		scoreText.text = currentScore.ToString();
	}

}
