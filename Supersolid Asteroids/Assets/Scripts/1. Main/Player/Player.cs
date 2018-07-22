using System.Collections;
using UnityEngine;

public class Player : BoundaryController {

	//============================================================
	// Constants:
	//============================================================

	private const float FORCE_STRENGTH = 10f;
	private const float ROTATION_SPEED = 300f;
	private readonly Vector3 ROTATION_DIRECTION = new Vector3(0, 0, 1);

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler PlayerDestroyedEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private Rigidbody2D playerRbody;
	[SerializeField] private Collider2D playerCollider;
	[SerializeField] private SpriteRenderer playerRenderer;
	[SerializeField] private PlayerShipBurner playerShipBurner;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GameObject bullet;
	[SerializeField] private GameObject bulletSpawnPoint;

	//============================================================
	// Private Fields:
	//============================================================

	private bool isInitialised;

	private Transform bulletContainer;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	public override void Update() {
		base.Update();

		// responsible for rotating the player
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(ROTATION_DIRECTION, Time.deltaTime * ROTATION_SPEED);
		}
		else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-ROTATION_DIRECTION, Time.deltaTime * ROTATION_SPEED);
		}

		// if we aren't initialised, we don't have a reference to the bullet container, so we can't shoot until we are
		if (!isInitialised) return;

		// allows the player to shoot their guns
		if (Input.GetKeyDown(KeyCode.Space)) {
			Instantiate(bullet, bulletSpawnPoint.transform.position, transform.localRotation, bulletContainer.transform);
		}
	}

	private void FixedUpdate() {

		// responsible for moving the player forwards
		if (Input.GetKey(KeyCode.W)) {
			playerRbody.AddForce((transform.up * FORCE_STRENGTH) * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {

		// alert those that are listening that we have been destroyed
		if (PlayerDestroyedEvent != null) {
			PlayerDestroyedEvent.Invoke();
		}

		// then destroy the gameobject
		Destroy(gameObject);
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void Init(Transform aBulletContainer, float invulnerabilityDuration = 0) {
		bulletContainer = aBulletContainer;

		// if an invulnerability duration is set, start the coroutine responsible for doing that
		if (invulnerabilityDuration > 0) {
			StartCoroutine(TemporaryInvulnerabilityCoroutine(invulnerabilityDuration));
		}

		// flag we are initialised
		isInitialised = true;
	}

	//============================================================
	// Coroutines:
	//============================================================

	private IEnumerator TemporaryInvulnerabilityCoroutine(float duration) {

		// calculate when our invulnerability will end for later use
		float invulnerabilityEnd = Time.time + duration;
		
		// disable our collider to prevent death
		playerCollider.enabled = false;

		// while we are in the invulnerable state, make our ship blink to make the player aware they are 
		while (Time.time < invulnerabilityEnd) {
			playerRenderer.enabled = !playerRenderer.enabled;
			yield return new WaitForSeconds(0.1f);
		}

		// re-enable our collider, and ensure our player renderer is enabled
		playerCollider.enabled = true;
		playerRenderer.enabled = true;
	}

}
