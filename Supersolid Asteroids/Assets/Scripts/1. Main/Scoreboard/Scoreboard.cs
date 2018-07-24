using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class Scoreboard {

	//============================================================
	// Constants:
	//============================================================

	public const int SCOREBOARD_SIZE = 5;

	//============================================================
	// Private Fields:
	//============================================================

	public List<Score> scores = new List<Score>(SCOREBOARD_SIZE);

	//============================================================
	// Constructors:
	//============================================================

	public Scoreboard() {
		SetupScoreboard();
	}

	//============================================================
	// Public Methods:
	//============================================================

	public void AddNewScore(Score score) {
		
		Score newScore = score;

		// iterate over each of the scores
		for (int i = 0; i < SCOREBOARD_SIZE; i++) {

			// check if the new score is greater than the existing one
			if (newScore.PlayerScore <= scores[i].PlayerScore) continue;

			// we have a better score, iterate over all scores moving them down the list
			for (int k = i; k < SCOREBOARD_SIZE; k++) {

				// record what score is currently in that position
				Score oldScore = scores[k];

				// overwrite what it was with the new score
				scores[k] = newScore;

				// then set the old score to new score for the next iteration.
				newScore = oldScore;
			}
			return;
		}
	}

	public void SetupScoreboard() {
		for (int i = 0; i < SCOREBOARD_SIZE; i++) {
			scores.Add(new Score("N/A", 0));
		}
	}

	public override string ToString() {

		StringBuilder toString = new StringBuilder();
		foreach (Score score in scores) {
			toString.Append(score + "\n");
		}
		return toString.ToString();
	}

}
