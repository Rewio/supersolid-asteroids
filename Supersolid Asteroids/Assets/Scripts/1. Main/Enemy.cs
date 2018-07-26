using System.Collections;
using UnityEngine;

public class Enemy : BoundaryController {

	//============================================================
	// Constants:
	//============================================================

	private const float DELAY_BETWEEN_SHOTS = 2f;

	private const float ENEMY_VELOCITY = 300f;
	private const float BULLET_VELOCITY = 2f;

	private const float MOVEMENT_CHANGE_DELAY  = 3f;
	private const float MOVEMENT_CHANGE_DEGREE = 45;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Bullet enemyBulletPrefab;
	[SerializeField] private Rigidbody2D enemyRigidbody;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GameObject deathParticles;

	public Transform testTransform;

	//============================================================
	// Private Fields:
	//============================================================

	private Transform playerTransform;

	private bool isInitialised;
	private float nextShootTime;

	private IEnumerator movementChangerEnumerator;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public override void Start() {
		base.Start();

		// randomly decide if the enemy will go from left to right, or right to left
		Vector3 direction = (Random.Range(0, 2) == 0) ? transform.right : -transform.right;

		enemyRigidbody.velocity = (direction * ENEMY_VELOCITY) * Time.fixedDeltaTime;
		StartCoroutine(movementChangerEnumerator = MovementChangerCoroutine());
	}

	public override void Update() {
		base.Update();

		// check if our shoot time has elapsed, and we have been initialised
		if (nextShootTime > Time.time || !isInitialised) return;

		// add a delay to our shots
		nextShootTime = Time.time + DELAY_BETWEEN_SHOTS;

		// calcuate the angle we need to fire from us towards the players ship
		Vector3    direction = transform.up * BULLET_VELOCITY;
		Quaternion rotation  = Quaternion.FromToRotation(direction, testTransform.position);

		// spawn in the bullet with it rotated towards the player
		Instantiate(enemyBulletPrefab, transform.position, rotation);
	}

	private void OnCollisionEnter2D(Collision2D col) {
		Destroy(gameObject);
	}

	private void OnDisable() {

		if (movementChangerEnumerator == null) return;

		StopCoroutine(movementChangerEnumerator);
		movementChangerEnumerator = null;
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void Init(Transform thePlayersTransform) {
		playerTransform = thePlayersTransform;
		isInitialised = true;
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
