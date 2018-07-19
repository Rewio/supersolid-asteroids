using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

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
	[SerializeField] private GameObject asteroidDeathParticles;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void Start() {

		for (int i = 0; i < 4; i++) {

			// select a random large asteroid to spawn from each of the large asteroids
			int asteroidToSpawn = Random.Range(0, largeAsteroids.Count);

			// select a random spawn location for the asteroid, based on the spawn locations, adding a slight offset for a bit more variety
			Vector2 asteroidSpawnLocation = (Vector2) asteroidSpawnPositions[Random.Range(0, asteroidSpawnPositions.Count)].position + (Random.insideUnitCircle * spawnOffsetMultiplier);

			// spawn the new asteroid with the previously generated values, then initialise it
			Asteroid newAsteroid = Instantiate(largeAsteroids[asteroidToSpawn], asteroidSpawnLocation, Quaternion.identity, spawnedAsteroidsContainer);
			newAsteroid.Init(Asteroid.AsteroidSizes.Large, spawnedAsteroidsContainer, mediumAsteroids[0], smallAsteroids[0], asteroidDeathParticles); // TODO: change the medium and small asteroid to spawn.
		}
	}

}
