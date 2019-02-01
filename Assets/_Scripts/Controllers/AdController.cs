using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*using GoogleMobileAds.Api;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;*/

public class AdController : MonoBehaviour {

	public string AdID;
	public string AppID;

	public static AdController Ins;

	void Start ()
	{
		Ins = this;
		/*Appodeal.initialize (AppID, Appodeal.REWARDED_VIDEO | Appodeal.NON_SKIPPABLE_VIDEO | Appodeal.INTERSTITIAL);*/
	}

	public void ShowAD ()
	{
		/*if (Appodeal.isLoaded (Appodeal.INTERSTITIAL)) {
			Appodeal.show (Appodeal.INTERSTITIAL);
		}*/
	}
}
