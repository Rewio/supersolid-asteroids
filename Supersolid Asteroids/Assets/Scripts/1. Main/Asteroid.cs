using UnityEngine;

public class Asteroid : BoundaryController {

	//============================================================
	// Constants:
	//============================================================

	private const float VELOCITY_MODIFIER = 1.25f;

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

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Rigidbody2D rbody;

	//============================================================
	// Private Fields:
	//============================================================

	private bool hasBeenInitialised;

	private AsteroidSizes asteroidSize;

	private Transform asteroidContainer;
	private Asteroid mediumAsteroidToSpawn;
	private Asteroid smallAsteroidToSpawn;

	private GameObject deathParticles;

	private Vector2 initialVelocity;

	//============================================================
	// Public Methods:
	//============================================================

	public void Init(AsteroidSizes anAsteroidSize, Transform anAsteroidContainer, Asteroid aMediumAsteroidToSpawn, Asteroid aSmallAsteroidToSpawn, 
	                 GameObject aDeathParticle, Vector2 anInitialVelocity) {
		asteroidSize      = anAsteroidSize;
		asteroidContainer = anAsteroidContainer;

		mediumAsteroidToSpawn = aMediumAsteroidToSpawn;
		smallAsteroidToSpawn  = aSmallAsteroidToSpawn;

		deathParticles = aDeathParticle;

		initialVelocity = anInitialVelocity;
		rbody.velocity  = initialVelocity;

		// flag that we have been initialised so things can begin to happen
		hasBeenInitialised = true;
	}

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public override void Update() {
		base.Update();

		// make sure we don't do anything before initialising, otherwise we may null reference
		if (!hasBeenInitialised) return;

		transform.Rotate(Vector3.forward, 1);
	}

	private void OnCollisionEnter2D(Collision2D col) {

		// we only want to split into more asteroids if we're either large or medium. Small asteroids do not split
		if (asteroidSize != AsteroidSizes.Small) {
			for (int i = 0; i < 2; i++) {

				// add a spawn offset to the new asteroids so they're not ontop of each other, and randomise their rotation
				Vector3 asteroidSpawnOffset = Random.insideUnitCircle * 0.5f;
				Quaternion asteroidStartingRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));

				// figure out which asteroid we're suppose to be spawning, then instantiate it
				Asteroid asteroidToSpawn = (asteroidSize == AsteroidSizes.Large) ? mediumAsteroidToSpawn : smallAsteroidToSpawn;
				Asteroid newAsteroid     = Instantiate(asteroidToSpawn, transform.position + asteroidSpawnOffset, asteroidStartingRotation, asteroidContainer);

				// initialise our new asteroid with the information it requires
				AsteroidSizes newAsteroidSize = (asteroidSize == AsteroidSizes.Large) ? AsteroidSizes.Medium : AsteroidSizes.Small;
				newAsteroid.Init(newAsteroidSize, asteroidContainer, mediumAsteroidToSpawn, smallAsteroidToSpawn, deathParticles, initialVelocity * VELOCITY_MODIFIER);
			}
		}

		// create our death particles then destroy the gameobject
		Instantiate(deathParticles, col.transform.position, Quaternion.identity, asteroidContainer);
		Destroy(gameObject);
	}

}
