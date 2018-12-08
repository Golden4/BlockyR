using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GameOverScreen : ScreenBase {

	public override void OnActivate ()
	{
		base.OnActivate ();

		if (AdController.Ins != null)
			AdController.Ins.ShowAD ();
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

}
