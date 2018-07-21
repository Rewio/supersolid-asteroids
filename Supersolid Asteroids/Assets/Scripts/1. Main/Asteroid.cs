using UnityEngine;

public class Asteroid : BoundaryController {

	//============================================================
	// Constants:
	//============================================================

	public delegate void AsteroidDestroyedDelegate(Asteroid asteroid, Vector2 asteroidDeathPosition);

	//============================================================
	// Events:
	//============================================================

	public static event AsteroidDestroyedDelegate AsteroidDestroyedEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Rigidbody2D rbody;
	[SerializeField] private GameObject deathParticles;

	//============================================================
	// Public Properties:
	//============================================================

	public AsteroidController.AsteroidSizes AsteroidSize { get; private set; }
	public Vector2 AsteroidVelocity { get; private set; }

	//============================================================
	// Private Fields:
	//============================================================

	private bool hasBeenInitialised;

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

		// make those who are listening aware that we have been destroyed
		if (AsteroidDestroyedEvent != null) {
			AsteroidDestroyedEvent.Invoke(this, transform.position);
		}

		// create our deathParticles particles then destroy the gameobject
		Instantiate(deathParticles, col.transform.position, transform.rotation);
		Destroy(gameObject);
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void Init(AsteroidController.AsteroidSizes anAsteroidSize, Vector2 anAsteroidVelocity) {
		AsteroidSize = anAsteroidSize;

		AsteroidVelocity = anAsteroidVelocity;
		rbody.velocity   = AsteroidVelocity;

		// flag that we have been initialised so things can begin to happen
		hasBeenInitialised = true;
	}

}
