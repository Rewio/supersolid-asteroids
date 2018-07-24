using System;

[Serializable]
public class Score {

	//============================================================
	// Constants:
	//============================================================

	private const string STRING_FORMAT = "Player: {0} - Score: {1}";

	//============================================================
	// Public Fields:
	//============================================================

	public string PlayerName;
	public int PlayerScore;

	//============================================================
	// Constructor:
	//============================================================

	public Score (string aPlayerName, int aPlayerScore) {
		PlayerName = aPlayerName;
		PlayerScore = aPlayerScore;
	}

	//============================================================
	// Public Methods:
	//============================================================

	public override string ToString() {
		string toString = string.Format(STRING_FORMAT, PlayerName, PlayerScore);
		return toString;
	}

}
