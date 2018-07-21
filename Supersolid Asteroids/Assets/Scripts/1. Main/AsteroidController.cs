using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int STARTING_ASTEROIDS    = 4;
	private const int ASTEROID_SPLIT_AMOUNT = 2;

	private const float ASTEROID_VELOCITY_MODIFIER  = 2f;
	private const float ASTEROID_SPAWN_MODIFIER     = 0.5f;
	private const float MAXIMUM_VELOCITY_ADJUSTMENT = 60f;

	//============================================================
	// Type Definitions:
	//============================================================

	public enum AsteroidSizes {
		Large,
		Medium,
		Small
	}

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private List<Asteroid> largeAsteroids;
	[SerializeField] private List<Asteroid> mediumAsteroids;
	[SerializeField] private List<Asteroid> smallAsteroids;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private List<Transform> asteroidSpawnPositions;
	[SerializeField] private Transform spawnedAsteroidsContainer;
	[SerializeField] private float spawnOffsetMultiplier;

	//============================================================
	// Private Fields:
	//============================================================

	private readonly List<Asteroid> trackedAsteroids = new List<Asteroid>();

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		Asteroid.AsteroidDestroyedEvent += Asteroid_AsteroidDestroyedEvent;
	}

	private void Start() {

		for (int i = 0; i < STARTING_ASTEROIDS; i++) {
			Vector2 asteroidSpawnLocation = (Vector2) asteroidSpawnPositions[Random.Range(0, asteroidSpawnPositions.Count)].position + (Random.insideUnitCircle * spawnOffsetMultiplier);
			Vector2 asteroidInitialVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

			SpawnNewAsteroid(AsteroidSizes.Large, asteroidSpawnLocation, asteroidInitialVelocity);
		}
	}

	private void OnDisable() {
		Asteroid.AsteroidDestroyedEvent -= Asteroid_AsteroidDestroyedEvent;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Asteroid_AsteroidDestroyedEvent(Asteroid asteroid, Vector2 asteroidDeathPosition) {

		// stop tracking the asteroid as it no-longer exists.
		trackedAsteroids.Remove(asteroid);

		// if the asteroid was small, we don't need to do anything atm
		if (asteroid.AsteroidSize == AsteroidSizes.Small) return;

		// get the new asteroid size based on the size of the one which died
		AsteroidSizes newAsteroidSize = asteroid.AsteroidSize + 1;

		// spawn the number of asteroids that the asteroid splits into on death
		for (int i = 0; i < ASTEROID_SPLIT_AMOUNT; i++) {

			// here, we are slightly adjusting the angle of velocity so the asteroids direction changes
			// this is randomised so the new asteroids head in their own directions
			Quaternion velocityAdjustment = Quaternion.AngleAxis(Random.Range(-MAXIMUM_VELOCITY_ADJUSTMENT, MAXIMUM_VELOCITY_ADJUSTMENT), asteroid.transform.forward);
			Vector3    adjustedVelocity   = velocityAdjustment * asteroid.AsteroidVelocity;

			SpawnNewAsteroid(newAsteroidSize, asteroidDeathPosition, adjustedVelocity * ASTEROID_VELOCITY_MODIFIER);
		}
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void SpawnNewAsteroid (AsteroidSizes newAsteroidSize, Vector2 newAsteroidSpawnPosition, Vector2 newAsteroidVelocity) {

		// find a random asteroid based on the size of asteroid we are to spawn
		Asteroid newAsteroid = null;
		switch (newAsteroidSize) {
			case AsteroidSizes.Large:
				newAsteroid = largeAsteroids[Random.Range(0, largeAsteroids.Count)];
				break;
			case AsteroidSizes.Medium:
				newAsteroid = mediumAsteroids[Random.Range(0, mediumAsteroids.Count)];
				break;
			case AsteroidSizes.Small:
				newAsteroid = smallAsteroids[Random.Range(0, smallAsteroids.Count)];
				break;
		}

		// if we reach this point and the asteroid is still null, something has gone wrong and it must be corrected
		if (newAsteroid == null) {
			throw new Exception("The asteroid was not initialised correctly.");
		}

		// generate some random values for spawn offset and starting rotation for a bit of variety with the asteroids
		Vector2    asteroidSpawnOffset      = Random.insideUnitCircle * ASTEROID_SPAWN_MODIFIER;
		Quaternion asteroidStartingRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));

		// instantiate the new asteroid, initialise it, then begin to track it
		newAsteroid = Instantiate(newAsteroid, newAsteroidSpawnPosition + asteroidSpawnOffset, asteroidStartingRotation, spawnedAsteroidsContainer);
		newAsteroid.Init(newAsteroidSize, newAsteroidVelocity);
		trackedAsteroids.Add(newAsteroid);
	}
}
