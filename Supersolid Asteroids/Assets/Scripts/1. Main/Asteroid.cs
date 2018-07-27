using UnityEngine;

public class Asteroid : BoundedEntity {

	//============================================================
	// Constants:
	//============================================================

	private const int ASTEROID_ROTATION_SPEED = 50;

	//============================================================
	// Delegates:
	//============================================================

	public delegate void AsteroidDestroyedDelegate(Asteroid asteroid, Vector2 asteroidDeathPosition);

	//============================================================
	// Events:
	//============================================================

	public static event AsteroidDestroyedDelegate AsteroidDestroyedEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Rigidbody2D rbody;
	[SerializeField] private Renderer asteroidRenderer;
	[SerializeField] private Collider2D asteroidCollider;
	[SerializeField] private AudioSource audioSource;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GameObject deathParticles;
	[SerializeField] private AudioClip deathSound;

	//============================================================
	// Private Fields:
	//============================================================

	private bool hasBeenInitialised;

	//============================================================
	// Public Properties:
	//============================================================

	public AsteroidController.AsteroidSizes AsteroidSize { get; private set; }
	public Vector2 AsteroidVelocity { get; private set; }

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected override void Update() {
		base.Update();

		// make sure we don't do anything before initialising, otherwise we may null reference
		if (!hasBeenInitialised) return;

		transform.Rotate(Vector3.forward, ASTEROID_ROTATION_SPEED * Time.deltaTime);
	}

	protected void OnCollisionEnter2D(Collision2D col) {

		// make those who are listening aware that we have been destroyed
		if (AsteroidDestroyedEvent != null) {
			AsteroidDestroyedEvent.Invoke(this, transform.position);
		}

		// play the asteroids death sound
		audioSource.PlayOneShot(deathSound);

		// hide the asteroid and disable the collider so it appears as if the asteroid has exploded
		asteroidRenderer.enabled = false;
		asteroidCollider.enabled = false;

		// create our deathParticles particles then destroy the gameobject after the death sound has finished
		Instantiate(deathParticles, col.transform.position, transform.rotation);
		Destroy(gameObject, deathSound.length);
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void Init(AsteroidController.AsteroidSizes anAsteroidSize, Vector2 anAsteroidVelocity) {
		AsteroidSize = anAsteroidSize;

		AsteroidVelocity = anAsteroidVelocity;
		rbody.velocity   = AsteroidVelocity * Time.fixedDeltaTime;

		// flag that we have been initialised so things can begin to happen
		hasBeenInitialised = true;
	}

}
