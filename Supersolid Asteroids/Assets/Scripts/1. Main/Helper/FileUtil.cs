using System.IO;
using UnityEngine;

public static class FileUtil {

	//============================================================
	// Constants:
	//============================================================

	private const string STORAGE_FOLDER  = "Scores";
	private const string SCORES_FILENAME = "scores.json";

	//============================================================
	// Private Fields:
	//============================================================

	private static string applicationDataPath = "";
	private static string storageFolderPath   = "";
	private static string scoresFilePath      = "";

	private static FileStream fileStream;

	//============================================================
	// Public Methods:
	//============================================================

	public static void SaveScoreboard(Scoreboard scoreboard) {

		// turn the scoreboard into json, then save it to disk
		string scoreboardJson = JsonUtility.ToJson(scoreboard);
		File.WriteAllText(scoresFilePath, scoreboardJson);
	}

	public static Scoreboard LoadScoreboard() {

		// read our scoreboard from disk in json format, then return it as a scoreboard object
		string savedScoreboard = File.ReadAllText(scoresFilePath);
		return JsonUtility.FromJson<Scoreboard>(savedScoreboard);
	}

	public static void Setup() {

		applicationDataPath = Application.persistentDataPath;
		storageFolderPath   = applicationDataPath + "/" + STORAGE_FOLDER;
		scoresFilePath      = storageFolderPath + "/" + SCORES_FILENAME;

		// make sure the application data path is created
		if (!Directory.Exists(applicationDataPath)) {
			Directory.CreateDirectory(applicationDataPath);
		}

		// ensure that we have a folder within that path for this game
		if (!Directory.Exists(storageFolderPath)) {
			Directory.CreateDirectory(storageFolderPath);
		}

		// and within that folder, the scores file exists for storing scores
		if (!File.Exists(scoresFilePath)) {
			fileStream = File.Create(scoresFilePath);

			// clear up all filestream data
			fileStream.Close();
			fileStream.Dispose();
			fileStream = null;
		}
	}

}
