using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*using GoogleMobileAds.Api;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;*/
using GoogleMobileAds.Api;
using System;

public class AdController : MonoBehaviour {
	 
	#if UNITY_ANDROID
	string adInterstitialID = "ca-app-pub-8878808814241755/6374843101";
	string adBannerID = "ca-app-pub-8878808814241755/1477739641";
	string adRewardedID = "ca-app-pub-8878808814241755/4399540162";
	string appID = "ca-app-pub-8878808814241755~7293420529";
	#else
	string adInterstitialID = "unexpected_platform";
	string adBannerID = "unexpected_platform";
	string adRewardedID = "unexpected_platform";
	string appID = "unexpected_platform";
	#endif

	private BannerView bannerView;
	private InterstitialAd interstitial;
	private RewardedAd rewardedAd;

	public static AdController Ins;

	void Start ()
	{
		Ins = this;

		MobileAds.Initialize (appID);

		RequestInterstitial ();
		RequestRewardedAd ();
	}

	public void ShowInterstitialAD ()
	{

		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
		}
	}

	public void ShowBannerAD ()
	{
		RequestBanner ();
	}

	public bool RewardedADLoaded ()
	{
		return this.rewardedAd.IsLoaded ();
	}

	public bool ShowRewardedAD ()
	{
		if (this.rewardedAd.IsLoaded ()) {
			this.rewardedAd.Show ();
			return true;
		}

		return false;
	}

	private void RequestBanner ()
	{
		bannerView = new BannerView (adBannerID, AdSize.Banner, AdPosition.Top);

		// Called when an ad request has successfully loaded.
		bannerView.OnAdLoaded += HandleBannerOnAdLoaded;
		// Called when an ad request failed to load.
		bannerView.OnAdFailedToLoad += HandleBannerOnAdFailedToLoad;
		// Called when an ad is clicked.
		bannerView.OnAdOpening += HandleBannerOnAdOpened;
		// Called when the user returned from the app after an ad click.
		bannerView.OnAdClosed += HandleBannerOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		bannerView.OnAdLeavingApplication += HandleBannerOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice (SystemInfo.deviceUniqueIdentifier.ToUpper ()).Build ();

		// Load the banner with the request.
		bannerView.LoadAd (request);
	}

	public void HandleBannerOnAdLoaded (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdLoaded event received");
	}

	public void HandleBannerOnAdFailedToLoad (object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print ("HandleFailedToReceiveAd event received with message: "
		+ args.Message);
	}

	public void HandleBannerOnAdOpened (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdOpened event received");
	}

	public void HandleBannerOnAdClosed (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdClosed event received");
	}

	public void HandleBannerOnAdLeavingApplication (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdLeavingApplication event received");
	}

	private void RequestInterstitial ()
	{
		// Initialize an InterstitialAd.
		this.interstitial = new InterstitialAd (adInterstitialID);

		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice (SystemInfo.deviceUniqueIdentifier.ToUpper ()).Build ();
		// Load the interstitial with the request.
		this.interstitial.LoadAd (request);
	}

	public void HandleOnAdLoaded (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad (object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print ("HandleFailedToReceiveAd event received with message: "
		+ args.Message);
	}

	public void HandleOnAdOpened (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdOpened event received");
	}

	public void HandleOnAdClosed (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdClosed event received");
	}

	public void HandleOnAdLeavingApplication (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleAdLeavingApplication event received");
	}

	public void RequestRewardedAd ()
	{

		this.rewardedAd = new RewardedAd (adRewardedID);

		// Called when an ad request has successfully loaded.
		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		// Called when an ad request failed to load.
		this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
		// Called when an ad is shown.
		this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
		// Called when an ad request failed to show.
		this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
		// Called when the user should be rewarded for interacting with the ad.
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		// Called when the ad is closed.
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().AddTestDevice (AdRequest.TestDeviceSimulator).AddTestDevice (SystemInfo.deviceUniqueIdentifier.ToUpper ()).Build ();
		// Load the rewarded ad with the request.
		this.rewardedAd.LoadAd (request);
	}

	public void HandleRewardedAdLoaded (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleRewardedAdLoaded event received");
	}

	public void HandleRewardedAdFailedToLoad (object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print (
			"HandleRewardedAdFailedToLoad event received with message: "
			+ args.Message);
	}

	public void HandleRewardedAdOpening (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow (object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print (
			"HandleRewardedAdFailedToShow event received with message: "
			+ args.Message);
	}

	public void HandleRewardedAdClosed (object sender, EventArgs args)
	{
		MonoBehaviour.print ("HandleRewardedAdClosed event received");
	}

	public void HandleUserEarnedReward (object sender, Reward args)
	{
		/*string type = args.Type;
		double amount = args.Amount;*/

		int coinAmount = 10;
		User.AddCoin (coinAmount);

		Vector3 fromPos = Vector3.zero;
		Vector3 toPos = CoinUI.Ins.coinImage.transform.position;

		Utility.CoinsAnimate (CoinUI.Ins, CoinUI.Ins.coinImage.gameObject, CoinUI.Ins.transform, coinAmount, fromPos, toPos, .5f, CoinUI.Ins.curve, () => {
			AudioManager.PlaySoundFromLibrary ("Coin");
		});

		/*MonoBehaviour.print (
			"HandleRewardedAdRewarded event received for "
			+ amount.ToString () + " " + type);*/
	}

}
