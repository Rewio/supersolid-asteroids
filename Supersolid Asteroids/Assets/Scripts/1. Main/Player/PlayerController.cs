using UnityEngine;

public class PlayerController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int NUM_STARTING_LIVES = 3;

	private const float PLAYER_RESPAWN_TIME = 2f;
	private const float PLAYER_INVULN_TIME  = 2f;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler NoLivesRemaining;

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

	[SerializeField]
	private int playersRemainingLives;

	private bool gameInProgress;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void OnEnable() {
		Player.PlayerDestroyedEvent += Player_PlayerDestroyed; 
	}

	private void OnDisable() {
		Player.PlayerDestroyedEvent -= Player_PlayerDestroyed;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Player_PlayerDestroyed() {

		// remove a life from the players remaining lives
		playersRemainingLives = playersRemainingLives - 1;

		// if the player has no lives remaining, signal that it is game over, then do nothing
		if (playersRemainingLives == 0) {
			if (NoLivesRemaining != null) {
				NoLivesRemaining.Invoke();
			}
			return;
		}

		// respawn the player after the desired time with invulnerability
		helper.InvokeActionDelayed(
			() => { SpawnPlayer(PLAYER_INVULN_TIME); }
			, PLAYER_RESPAWN_TIME);
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void StartGame() {

		// if the game is in progress, we dont want to any of this
		if (gameInProgress) return;

		// flag that we are current playing
		gameInProgress = true;

		// set the players starting lives
		playersRemainingLives = NUM_STARTING_LIVES;

		// bring the player into the game
		SpawnPlayer();
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void SpawnPlayer(float invulnerabilityDuration = 0) {

		// respawn the player in the centre of the play area
		Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
		player.Init(bulletContainer, invulnerabilityDuration);
	}

}