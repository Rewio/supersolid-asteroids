using UnityEngine;

public class PlayerController : BoundaryController {

	//============================================================
	// Constants:
	//============================================================

	private const float FORCE_STRENGTH = 10f;
	private const float ROTATION_SPEED = 300f;
	private readonly Vector3 ROTATION_DIRECTION = new Vector3(0, 0, 1);

	//============================================================
	// Inspector Variables:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private GameObject player;
	[SerializeField] private Rigidbody2D playerRigidbody;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private GameObject playerBullet;
	[SerializeField] private GameObject bulletSpawnPoint;
	[SerializeField] private GameObject bulletContainer;


	//============================================================
	// Unity Lifecycle:
	//============================================================

	public override void Update() {
		base.Update();

		// responsible for rotating the player
		if (Input.GetKey(KeyCode.A)) {
			player.transform.Rotate(ROTATION_DIRECTION, Time.deltaTime * ROTATION_SPEED);
		}
		else if (Input.GetKey(KeyCode.D)) {
			player.transform.Rotate(-ROTATION_DIRECTION, Time.deltaTime * ROTATION_SPEED);
		}

		// allows the player to shoot their guns
		if (Input.GetKeyDown(KeyCode.Space)) {
			Instantiate(playerBullet, bulletSpawnPoint.transform.position, player.transform.localRotation, bulletContainer.transform);
		}
	}

	private void FixedUpdate() {

		// responsible for moving the player forwards
		if (Input.GetKey(KeyCode.W)) {
			playerRigidbody.AddForce((player.transform.up * Time.deltaTime) * FORCE_STRENGTH, ForceMode2D.Impulse);
		}
	}
}