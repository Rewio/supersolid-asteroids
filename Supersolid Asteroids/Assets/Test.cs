using UnityEngine;

public class Test : MonoBehaviour {

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private Scoreboard scoreboard;
	private Scoreboard newScoreboard;

	private string json = "";

	private void Start() {
		scoreboard = new Scoreboard();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			print(scoreboard);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0)) {

			string name = "Bob" + Random.Range(0, 200);

			Score newScore = new Score(name, Random.Range(0, 20));
			print(newScore);

			scoreboard.AddNewScore(newScore);
			print(scoreboard);
		}

		if (Input.GetKeyDown(KeyCode.Mouse1)) {
			json = JsonUtility.ToJson(scoreboard);
			print(json);
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			newScoreboard = JsonUtility.FromJson<Scoreboard>(json);
			print(newScoreboard);
		}
	}

}
