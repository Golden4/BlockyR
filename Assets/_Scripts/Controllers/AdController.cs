using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdController : MonoBehaviour {

	public string AdID;
	public string AppID;

	public static AdController Ins;

	public InterstitialAd ad;

	void Start ()
	{
		Ins = this;
		MobileAds.Initialize (AppID);

		ad = new InterstitialAd (AdController.Ins.AdID);
		ad.LoadAd (new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice (SystemInfo.deviceUniqueIdentifier.ToUpper ()).Build ());
	}

	public void ShowAD ()
	{
		//if (ad.IsLoaded ()) {
		ad.Show ();
		//}
	}

	void Update ()
	{
		
	}
}
