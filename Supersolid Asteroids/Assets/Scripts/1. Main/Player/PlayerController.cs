﻿using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const int NUM_STARTING_LIVES = 3;

	private const float PLAYER_RESPAWN_TIME = 2f;
	private const float PLAYER_INVULN_TIME  = 2f;

	private const float LIFE_DEDUCTION_COOLDOWN = 1f;

	//============================================================
	// Delegates:
	//============================================================

	public delegate void RemainingLivesUpdateDelegate(int remainingLives);
	public delegate void NewPlayerObjectDelegate(Transform newPlayerTransform);

	//============================================================
	// Events:
	//============================================================

	public static event RemainingLivesUpdateDelegate RemainingLivesUpdate;
	public static event Helper.EventHandler NoLivesRemaining;
	public static event NewPlayerObjectDelegate NewPlayer;

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

	private float timeLastLifeRemoved;

	private Player player;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnEnable() {
		Player.PlayerDestroyed += Player_PlayerDestroyed; 
		GameController.NewGame += GameController_NewGame;
		GuiScoreView.NewLifeEarned  += GuiScoreView_NewLifeEarned;
	}

	protected void OnDisable() {
		Player.PlayerDestroyed -= Player_PlayerDestroyed;
		GameController.NewGame -= GameController_NewGame;
		GuiScoreView.NewLifeEarned  -= GuiScoreView_NewLifeEarned;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Player_PlayerDestroyed() {

		// slight edge case where player can collide with two asteroids simultaneously
		// this prevents the loss of two lives in that event
		if (Time.time > timeLastLifeRemoved) {

			// add to the timer to prevent extra loss
			timeLastLifeRemoved = Time.time + LIFE_DEDUCTION_COOLDOWN;

			// remove a life from the players remaining lives
			playersRemainingLives = playersRemainingLives - 1;

			// tell anyone listening that the players lives have updated
			if (RemainingLivesUpdate != null) {
				RemainingLivesUpdate.Invoke(playersRemainingLives);
			}
		}

		// if the player has no lives remaining, signal that it is game over, then do nothing
		if (playersRemainingLives <= 0) {
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
	
	private void GameController_NewGame() {

		// reset the players remaining lives
		playersRemainingLives = NUM_STARTING_LIVES;

		// spawn them into the game
		SpawnPlayer();
	}

	private void GuiScoreView_NewLifeEarned() {
		playersRemainingLives = playersRemainingLives + 1;
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void SpawnPlayer(float invulnerabilityDuration = 0) {

		// respawn the player in the centre of the play area
		player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
		player.Init(bulletContainer, invulnerabilityDuration);

		// signal a new player has spawned, and to update transforms if necessary
		if (NewPlayer != null) {
			NewPlayer.Invoke(player.transform);
		}
	}

}