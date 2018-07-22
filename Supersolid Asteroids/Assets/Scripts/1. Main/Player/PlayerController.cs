using UnityEngine;

public class PlayerController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int NUM_STARTING_LIVES = 3;

	private const string PLAYER_NAME = "Player";

	private const float PLAYER_RESPAWN_TIME = 2f;
	private const float PLAYER_INVULN_TIME  = 2f;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler GameOverEvent;

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private Helper helper;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private Player playerPrefab;
	[SerializeField] private Transform bulletContainer;

	//============================================================
	// Private Fields:
	//============================================================

	private int playersRemainingLives;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		Player.PlayerDestroyedEvent += Player_PlayerDestroyed; 
	}

	private void Start() {

		// set the players starting lives
		playersRemainingLives = NUM_STARTING_LIVES;

		// spawn the player into the world
		SpawnPlayer();
	}

	private void OnDisable() {
		Player.PlayerDestroyedEvent -= Player_PlayerDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Player_PlayerDestroyed() {

		// remove a life from the players remaining lives
		playersRemainingLives--;

		// if the player has no lives remaining, signal that it is game over, then do nothing
		if (playersRemainingLives == 0) {
			if (GameOverEvent != null) {
				GameOverEvent.Invoke();
			}
			return;
		}

		// respawn the player after the desired time with invulnerability
		helper.InvokeActionDelayed(
			() => { SpawnPlayer(PLAYER_INVULN_TIME); }
			, PLAYER_RESPAWN_TIME);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void SpawnPlayer(float invulnerabilityDuration = 0) {

		// respawn the player in the centre of the play area
		Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
		player.name   = PLAYER_NAME;
		player.Init(bulletContainer, invulnerabilityDuration);
	}

}