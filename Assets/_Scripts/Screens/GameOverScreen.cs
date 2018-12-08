using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GameOverScreen : ScreenBase {


	InterstitialAd ad;

	public override void OnActivate ()
	{
		base.OnActivate ();
		ad = new InterstitialAd (AdController.Ins.AdID);
		AdRequest request = new AdRequest.Builder ().Build ();
		ad.LoadAd (request);
		if (ad.IsLoaded ()) {
			ad.Show ();
		}
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
