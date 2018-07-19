using UnityEngine;

public class Asteroid : BoundaryController {

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

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Rigidbody2D rbody;

	[SerializeField] private float initialVelocityModifier;
	[SerializeField] private float velocityModifierIncrement;

	[SerializeField] private Asteroid mediumAsteroidToSpawn;
	[SerializeField] private Asteroid smallAsteroidToSpawn;

	//============================================================
	// Private Fields:
	//============================================================

	private bool hasBeenInitialised;

	private AsteroidSizes asteroidSize;

	private Transform asteroidContainer;

	//============================================================
	// Public Methods:
	//============================================================

	public void Init(AsteroidSizes anAsteroidSize, Transform anAsteroidContainer, Asteroid aMediumAsteroidToSpawn, Asteroid aSmallAsteroidToSpawn) {
		asteroidSize      = anAsteroidSize;
		asteroidContainer = anAsteroidContainer;

		mediumAsteroidToSpawn = aMediumAsteroidToSpawn;
		smallAsteroidToSpawn  = aSmallAsteroidToSpawn;

		// flag that we have been initialised so things can begin to happen
		hasBeenInitialised = true;
	}

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public override void Start() {
		base.Start();

		rbody.velocity = Random.insideUnitCircle * initialVelocityModifier;
	}

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
				Vector3  positionOffset  = Random.insideUnitCircle;
				Asteroid asteroidToSpawn = (asteroidSize == AsteroidSizes.Large) ? mediumAsteroidToSpawn : smallAsteroidToSpawn;
				Asteroid newAsteroid     = Instantiate(asteroidToSpawn, gameObject.transform.localPosition + positionOffset, Quaternion.identity);

				AsteroidSizes newAsteroidSize = (asteroidSize == AsteroidSizes.Large) ? AsteroidSizes.Medium : AsteroidSizes.Small;
				newAsteroid.Init(newAsteroidSize, gameObject.transform.parent, mediumAsteroidToSpawn, smallAsteroidToSpawn);
			}
		}

		Destroy(gameObject);
	}

}
