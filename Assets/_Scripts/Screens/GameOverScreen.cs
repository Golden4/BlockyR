using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : ScreenBase {

	[SerializeField] Button continueAdBtn;

	public override void OnActivate ()
	{
		base.OnActivate ();

		continueAdBtn.onClick.RemoveAllListeners ();
		continueAdBtn.onClick.AddListener (RespawnPlayer);
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

}
