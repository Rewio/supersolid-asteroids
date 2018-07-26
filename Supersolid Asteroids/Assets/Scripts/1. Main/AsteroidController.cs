using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const float ASTEROID_VELOCITY_MODIFIER  = 1.5f;
	private const float ASTEROID_MAXIMUM_VELOCITY   = 100f;
	private const float ASTEROID_SPAWN_MODIFIER     = 0.5f;
	private const float MAXIMUM_VELOCITY_ADJUSTMENT = 45f;

	private const int DEFAULT_ASTEROID_SPLIT_AMOUNT = 2;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler AllAsteroidsDestroyedEvent;

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

	//============================================================
	// Private Fields:
	//============================================================

	private readonly List<Asteroid> trackedAsteroids = new List<Asteroid>();

	private int asteroidSplitAmount = 2;

	private List<Transform> spawnTransformsCopy;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		Asteroid.AsteroidDestroyedEvent += Asteroid_AsteroidDestroyed;
		GameController.NewGameEvent     += GameController_NewGame;
	}

	private void OnDisable() {
		Asteroid.AsteroidDestroyedEvent -= Asteroid_AsteroidDestroyed;
		GameController.NewGameEvent     -= GameController_NewGame;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Asteroid_AsteroidDestroyed(Asteroid asteroid, Vector2 asteroidDeathPosition) {

		// stop tracking the asteroid as it no-longer exists
		trackedAsteroids.Remove(asteroid);

		// if the last asteroid was destroyed, send an alert so the next wave can begin
		if (trackedAsteroids.Count == 0 && asteroid.AsteroidSize == AsteroidSizes.Small) {
			if (AllAsteroidsDestroyedEvent != null) {
				AllAsteroidsDestroyedEvent.Invoke();
			}
			return;
		}

		// if the asteroid was small, we don't need to do anything atm
		if (asteroid.AsteroidSize == AsteroidSizes.Small) return;

		// get the new asteroid size based on the size of the one which died
		AsteroidSizes newAsteroidSize = asteroid.AsteroidSize + 1;

		// spawn the number of asteroids that the asteroid splits into on death
		for (int i = 0; i < asteroidSplitAmount; i++) {

			// here, we are slightly adjusting the angle of velocity so the asteroids direction changes
			// this is randomised so the new asteroids head in their own directions
			Quaternion velocityAdjustment = Quaternion.AngleAxis(Random.Range(-MAXIMUM_VELOCITY_ADJUSTMENT, MAXIMUM_VELOCITY_ADJUSTMENT), asteroid.transform.forward);
			Vector3    adjustedVelocity   = velocityAdjustment * asteroid.AsteroidVelocity;

			SpawnNewAsteroid(newAsteroidSize, asteroidDeathPosition, adjustedVelocity * ASTEROID_VELOCITY_MODIFIER);
		}
	}

	private void GameController_NewGame() {

		// reset the split amount
		asteroidSplitAmount = DEFAULT_ASTEROID_SPLIT_AMOUNT;

		// create a local copy of the old asteroids to prevent enumeration errors
		List<Asteroid> oldAsteroids = trackedAsteroids;

		// iterate over each of the old asteroids destroying them
		foreach (Asteroid asteroid in oldAsteroids) {
			Destroy(asteroid.gameObject);
		}

		// clear all of the tracked asteroids, we have new ones to track
		trackedAsteroids.Clear();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void SpawnAsteroids(int newAsteroidSplitAmount, int numberOfAsteroids) {

		// update the asteroid split amount if it has changed
		if (newAsteroidSplitAmount > asteroidSplitAmount) {
			asteroidSplitAmount = newAsteroidSplitAmount;
		}

		// create a copy of the spawn positions
		spawnTransformsCopy = new List<Transform>(asteroidSpawnPositions);

		for (int i = 0; i < numberOfAsteroids; i++) {

			// if our copied list is empty, copy the list over again
			if (spawnTransformsCopy.Count == 0) {
				spawnTransformsCopy = new List<Transform>(asteroidSpawnPositions);
			}

			// generate a random spawn position
			int randomSpawnPosition = Random.Range(0, spawnTransformsCopy.Count);

			// generate a random starting position and starting velocity for the new asteroid
			Vector2 spawnPosition = spawnTransformsCopy[randomSpawnPosition].position;
			Vector2 startingVelocity = new Vector2(Random.Range(-ASTEROID_MAXIMUM_VELOCITY, ASTEROID_MAXIMUM_VELOCITY),
			                                       Random.Range(-ASTEROID_MAXIMUM_VELOCITY, ASTEROID_MAXIMUM_VELOCITY));

			// remove that spawn position from the copied list so it not repeated until all positions are used
			spawnTransformsCopy.RemoveAt(randomSpawnPosition);

			SpawnNewAsteroid(AsteroidSizes.Large, spawnPosition, startingVelocity);
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
