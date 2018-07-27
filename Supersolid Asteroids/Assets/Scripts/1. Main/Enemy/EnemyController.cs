using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const float ENEMY_SPAWN_DELAY     = 10f;
	private const float ENEMY_NEW_SPAWN_DELAY = 4f;

	//============================================================
	// Events:
	//============================================================

	public static event Helper.EventHandler AllEnemiesDestroyed;

	//============================================================
	// Inspector Variables:
	//============================================================

	[SerializeField] private Helper helper;

	[Space(Helper.INSPECTOR_SPACE)]

	[SerializeField] private List<Transform> enemySpawnPositions;
	[SerializeField] private Transform spawnedEnemyContainer;
	[SerializeField] private Enemy enemyPrefab;

	//============================================================
	// Private Fields:
	//============================================================

	private Enemy trackedEnemy;
	private Transform playerTransform;

	private int enemiesToSpawnThisWave;

	//============================================================
	// Unity Lifecycle:
	//============================================================

	protected void OnEnable() {
		Enemy.EnemyDestroyed   += Enemy_EnemyDestroyed;
		GameController.NewGame += GameController_NewGame;
		PlayerController.NewPlayer += PlayerController_NewPlayer;
	}

	protected void OnDisable() {
		Enemy.EnemyDestroyed   -= Enemy_EnemyDestroyed;
		GameController.NewGame -= GameController_NewGame;
		PlayerController.NewPlayer -= PlayerController_NewPlayer;
	}

	//============================================================
	// Event Handlers:
	//============================================================

	private void Enemy_EnemyDestroyed() {

		// if we have more to spawn, spawn one after a delay
		if (enemiesToSpawnThisWave > 0) {
			SpawnEnemy(ENEMY_NEW_SPAWN_DELAY);
		}

		// otherwise, signal that all the enemies have been destroyed
		else {
			if (AllEnemiesDestroyed != null) {
				AllEnemiesDestroyed.Invoke();
			}
		}
	}

	private void GameController_NewGame() {

		// if the enemy is still alive after the game has restarted, destroy them
		if (trackedEnemy == null) return;
		Destroy(trackedEnemy.gameObject);
	}

	private void PlayerController_NewPlayer(Transform newPlayerTransform) {
		playerTransform = newPlayerTransform;

		// the enemy might have died, lets make sure they're alive and well
		if (trackedEnemy == null) return;
		trackedEnemy.PlayerTransform = playerTransform;
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void SetupEnemySpawning(int enemiesToSpawn) {
		enemiesToSpawnThisWave = enemiesToSpawn;

		// spawn our enemy after a delay
		SpawnEnemy(ENEMY_SPAWN_DELAY);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void SpawnEnemy(float spawnDelay = 0) {

		// decrement our enemies to spawn counter
		enemiesToSpawnThisWave = enemiesToSpawnThisWave - 1;

		// get a random spawn position for our new enemy
		Vector3 spawnPosition = enemySpawnPositions[Random.Range(0, enemySpawnPositions.Count)].position;

		// spawn our new enemy after a delay if one is set
		helper.InvokeActionDelayed(() => {
			Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, spawnedEnemyContainer);
			enemy.PlayerTransform = playerTransform;
			trackedEnemy = enemy;
		}, spawnDelay);
	}

}
