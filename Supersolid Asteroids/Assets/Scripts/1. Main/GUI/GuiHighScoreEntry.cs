using UnityEngine;
using UnityEngine.UI;

public class GuiHighScoreEntry : GuiView {

	//============================================================
	// Constants:
	//============================================================

	private const float VIEW_VERTICAL_OFFSET = 50f;

	//============================================================
	// Inspector Variable:
	//============================================================

	[Space(Helper.INSPECTOR_SPACE_BIG)]

	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private Text playerNameText;
	[SerializeField] private Text playerScoreText;

	//============================================================
	// Public Methods:
	//============================================================

	public void SetRectOffset(float offsetMultiplier) {
		rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -VIEW_VERTICAL_OFFSET * offsetMultiplier);
	}

	public void SetPlayerName(string playerName) {
		playerNameText.text = playerName;
	}

	public void SetPlayerScore(int playerScore) {
		playerScoreText.text = playerScore.ToString();
	}

}
