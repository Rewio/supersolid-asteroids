using System.IO;
using UnityEngine;

public class FileUtil : MonoBehaviour {

	//============================================================
	// Constants:
	//============================================================

	private const string STORAGE_FOLDER  = "Scores";
	private const string SCORES_FILENAME = "scores.json";

	//============================================================
	// Private Fields:
	//============================================================

	private string applicationDataPath = "";
	private string storageFolderPath   = "";
	private string scoresFilePath      = "";

	//============================================================
	// Unity Lifecycle:
	//============================================================

	private void Start() {
		applicationDataPath = Application.persistentDataPath;
		storageFolderPath   = applicationDataPath + "/" + STORAGE_FOLDER;
		scoresFilePath      = storageFolderPath + "/" + SCORES_FILENAME;
	}

	private void Update() {

		if (Input.GetKeyDown(KeyCode.Space)) {
			Setup();
		}

	}

	//============================================================
	// Public Methods:
	//============================================================

	public void WriteToFile(string contentsToWrite) {
		File.WriteAllText(scoresFilePath, contentsToWrite);
	}

	public string ReadFromFile() {
		return File.ReadAllText(scoresFilePath);
	}

	//============================================================
	// Private Methods:
	//============================================================

	private void Setup() {

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
			File.Create(scoresFilePath);
		}
	}

}
