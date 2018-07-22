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

		// record the times for when the object was spawned and when it is due to be destroyed
		spawnTime = Time.time;
		deathTime = spawnTime + lifetime;

		// add some initial velocity to the bullet so it can begin moving
		rbody.velocity = (transform.up * velocityModifier) * Time.fixedDeltaTime;
	}

	private void Update() {

		// if our lifetime has elapsed, destroy ourself
		if (Time.time > deathTime) {
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D col) {

		// we've collided with something, we must be destroyed
		Destroy(gameObject);
	}

}
