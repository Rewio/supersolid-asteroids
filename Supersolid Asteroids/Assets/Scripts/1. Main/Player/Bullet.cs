using UnityEngine;

public class Bullet : MonoBehaviour {

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private float lifetime;
	[SerializeField] private float velocityModifier;
	[SerializeField] private Rigidbody2D rbody;

	//============================================================
	// Private Fields:
	//============================================================

	private float spawnTime;
	private float deathTime;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void Start() {

		// record the time which we spawned the object, and the time when it should die
		spawnTime = Time.time;
		deathTime = spawnTime + lifetime;

		// add some initial velocity to the bullet so it can begin moving
		rbody.velocity = transform.up * velocityModifier;
	}

	private void Update() {

		// if the objects lifetime has elapsed, destroy it
		if (Time.time > deathTime) {
			Destroy(gameObject);
		}
	}

}
