using System.Collections;
using UnityEngine;

public class Enemy : BoundedEntity {

	//============================================================
	// Constants:
	//============================================================

	private const float DELAY_BETWEEN_SHOTS = 4f;

	private const float ENEMY_VELOCITY = 150f;
	private const float BULLET_VELOCITY = 500f;

	private const float MOVEMENT_CHANGE_DELAY  = 3f;
	private const float MOVEMENT_CHANGE_DEGREE = 45;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler EnemyDestroyed;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Bullet enemyBulletPrefab;
	[SerializeField] private Rigidbody2D enemyRigidbody;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private AudioSource audioSource;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GameObject deathParticles;

	//============================================================
	// Public Properties:
	//============================================================

	public Transform PlayerTransform { get; set; }

	//============================================================
	// Private Fields:
	//============================================================

	private float nextShootTime;

	private IEnumerator movementChangerEnumerator;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnEnable() {
		GameController.GameOver += GameController_GameOver;
	}

	protected override void Start() {
		base.Start();

		// randomly decide if the enemy will go from left to right, or right to left
		Vector3 direction = (Random.Range(0, 2) == 0) ? transform.right : -transform.right;
		enemyRigidbody.velocity = (direction * ENEMY_VELOCITY) * Time.fixedDeltaTime;

		// add an initial delay to the shooting
		nextShootTime = Time.time + DELAY_BETWEEN_SHOTS;

		// start the coroutine responsible for changing movement
		StartCoroutine(movementChangerEnumerator = MovementChangerCoroutine());
	}

	protected override void Update() {
		base.Update();

		// check if our shoot time has elapsed, and we have been initialised
		if (nextShootTime > Time.time || PlayerTransform == null) return;

		// add a delay to our shots
		nextShootTime = Time.time + DELAY_BETWEEN_SHOTS;

		// create the shot vector for the bullet to use as its velocity
		Vector3 shotDirection = PlayerTransform.position - transform.position;

		// spawn in the bullet with it rotated towards the player
		Bullet bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
		bullet.Init(shotDirection.normalized * BULLET_VELOCITY);
	}

	protected void OnCollisionEnter2D(Collision2D col) {

		if (EnemyDestroyed != null) {
			EnemyDestroyed.Invoke();
		}

		Instantiate(deathParticles, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	protected void OnDisable() {
		GameController.GameOver -= GameController_GameOver;

		if (movementChangerEnumerator == null) return;

		StopCoroutine(movementChangerEnumerator);
		movementChangerEnumerator = null;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void GameController_GameOver() {

		// stop the audio source from playing, it gets annoying
		audioSource.Stop();
	}

	//============================================================
	// Coroutines:
	//============================================================

	private IEnumerator MovementChangerCoroutine() {

		// create the waitforseconds object here, instead of each time
		WaitForSeconds secondsToWait = new WaitForSeconds(MOVEMENT_CHANGE_DELAY);

		while (movementChangerEnumerator != null) {

			// wait to start with, we dont want to immediately change direction
			yield return secondsToWait;

			// randomly decide which direction the change will be
			float angleAdjustment = (Random.Range(0, 2) == 0) ? MOVEMENT_CHANGE_DEGREE : -MOVEMENT_CHANGE_DEGREE;

			// calculate our new velocity with the direction change
			Quaternion velocityAdjustment = Quaternion.AngleAxis(angleAdjustment, transform.forward);
			Vector3    adjustedVelocity   = velocityAdjustment * enemyRigidbody.velocity;

			// set our new velocity
			enemyRigidbody.velocity = adjustedVelocity;
		}
	}

}
