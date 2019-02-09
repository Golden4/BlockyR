using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : ScreenBase {

	[SerializeField] Transform continueAdPanel;
	[SerializeField] Transform openBoxPanel;

	[SerializeField] Button continueAdBtn;
	[SerializeField] Button openBoxBtn;
	public Slider coinSlider;
	public Text needCoinText;

	public override void Init ()
	{
		base.Init ();
		continueAdBtn.onClick.RemoveAllListeners ();
		continueAdBtn.onClick.AddListener (RespawnPlayer);
		openBoxBtn.onClick.RemoveAllListeners ();
		openBoxBtn.onClick.AddListener (OpenBoxBtn);
	}

	public override void OnActivate ()
	{
		base.OnActivate ();

		if (!User.GetInfo.AllCharactersBought ()) {
			openBoxPanel.gameObject.SetActive (true);
			if (User.HaveCoin (PrizeScreen.GetBoxPrise ())) {
				openBoxBtn.gameObject.SetActive (true);
				coinSlider.gameObject.SetActive (false);
			} else {
				openBoxBtn.gameObject.SetActive (false);
				coinSlider.gameObject.SetActive (true);
				coinSlider.value = (float)User.Coins / PrizeScreen.GetBoxPrise ();
				needCoinText.text = User.Coins + "/" + PrizeScreen.GetBoxPrise ();
			}
		} else {
			openBoxPanel.gameObject.SetActive (false);
		}

		GUIAnimSystem.Instance.MoveIn (transform, true);

	}

	public override void OnCleanUp ()
	{
		base.OnCleanUp ();
	}

	public override void OnDeactivate ()
	{
		base.OnDeactivate ();

	}

	public void RestartLevel ()
	{
		SceneController.RestartLevel ();
	}

	public void RetryGame ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.UI);

		Player.Ins.Retry ();
	}

	void RespawnPlayer ()
	{
		if (AdController.Ins != null)
			AdController.Ins.ShowAD ();
	}

	public void OpenBoxBtn ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.Prize);
	}

}
