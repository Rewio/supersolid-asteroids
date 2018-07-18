﻿using UnityEngine;

public class PlayerController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const float FORCE_STRENGTH = 0.25f;

	private const float ROTATION_ANGLE = 5;
	private readonly Vector3 ROTATION_DIRECTION = new Vector3(0, 0, 1);

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(10)]
	[SerializeField] private GameObject player;
	[SerializeField] private Rigidbody2D playerRigidbody;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void Update() {

		// debug key to reset the players position, velocity and angular velocity.
		if (Input.GetKeyDown(KeyCode.Return)) {
			player.transform.position   = Vector3.zero;
			playerRigidbody.velocity    = Vector2.zero;
		}

		// responsible for rotating the player
		if (Input.GetKey(KeyCode.A)) {
			player.transform.Rotate(ROTATION_DIRECTION, ROTATION_ANGLE);
		}
		else if (Input.GetKey(KeyCode.D)) {
			player.transform.Rotate(-ROTATION_DIRECTION, ROTATION_ANGLE);
		}
	}

	private void FixedUpdate() {

		// responsible for moving the player forwards
		if (Input.GetKey(KeyCode.W)) {
			playerRigidbody.AddForce(player.transform.up * FORCE_STRENGTH, ForceMode2D.Impulse);
		}
	}
}