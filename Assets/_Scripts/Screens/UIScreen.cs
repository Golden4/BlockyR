using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreen : ScreenBase {
	public static UIScreen Ins;

	int _topScore = -1;

	public int topScore {
		get {
			if (_topScore == -1) {
				if (PlayerPrefs.HasKey ("TopScore"))
					_topScore = PlayerPrefs.GetInt ("TopScore");
				else
					_topScore = 0;
			}

			return _topScore;
		}

		private set {
			_topScore = value;
			PlayerPrefs.SetInt ("TopScore", value);
		}
	}

	public Text topScoreText;

	public int score = 0;
	public Text scoreText;

	public Image fireAlert;
	public Text fireAlertText;

	public GameObject inputsMap;

	public Button abilityBtn;

	public override void Init ()
	{
		Ins = this;
		Player.OnPlayerDie += SetTopScore;
		fireAlert.gameObject.SetActive (false);
	}

	public override void OnCleanUp ()
	{
		Player.OnPlayerDie -= SetTopScore;
	}

	void SetTopScore ()
	{
		if (topScore < score)
			topScore = score;
	}

	bool gameStarted;

	public override void OnActivate ()
	{
		topScoreText.text = "Top " + topScore.ToString ();

		if (!gameStarted)
			Game.OnGameStartedCall ();

		gameStarted = true;

		if (Player.Ins.ability == null) {
			abilityBtn.gameObject.SetActive (false);
		} else {
			abilityBtn.gameObject.SetActive (true);
			abilityBtn.onClick.RemoveAllListeners ();
			abilityBtn.onClick.AddListener (AbilityBtn);
		}
	}

	void AbilityBtn ()
	{
		if (Player.Ins.ability != null)
			Player.Ins.ability.Use ();
	}

	void Update ()
	{
		if (Fire.Ins != null && !Player.isWaitingOnStart) {
			ShowFireDistance (Fire.Ins.distanceToPlayer);
		}

	}

	public void UpdateScore (int curPos)
	{
		if (curPos > score) {
			score = curPos;
			scoreText.text = curPos.ToString ();
		}
	}

	void ShowFireDistance (float distance)
	{
		if (distance < 3f) {
			fireAlert.gameObject.SetActive (true);
			fireAlertText.text = (distance).ToString ("F1") + "m";
		} else {
			fireAlert.gameObject.SetActive (false);
		}
	}

	public void ShowInputsMap (bool activate)
	{
		Time.timeScale = (activate) ? 0 : 1;

		inputsMap.SetActive (activate);
	}
}
