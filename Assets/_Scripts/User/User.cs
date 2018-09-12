using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class User {

	const string coinsKey = "coins";
	static int _coins;

	static bool coinsLoaded = false;

	public static int Coins {
		get {
			LoadCoins ();
			return _coins;
		}
		private set {

			LoadCoins ();

			SaveCoins (value);

			if (OnCoinChangedEvent != null)
				OnCoinChangedEvent (_coins, value);

			_coins = value;

		}
	}

	public static void AddCoin (int value)
	{
		Coins += value;
	}

	static void LoadCoins ()
	{
		if (!coinsLoaded) {
			coinsLoaded = true;

			if (PlayerPrefs.HasKey (coinsKey))
				_coins = PlayerPrefs.GetInt (coinsKey);
			
		}
	}

	static void SaveCoins (int coinsCount)
	{
		PlayerPrefs.SetInt (coinsKey, coinsCount);
		PlayerPrefs.Save ();
	}

	public static event Action<int,int> OnCoinChangedEvent;

	static UserInfos dataUser;

	static bool Loaded;

	public static UserInfos GetInfo {
		get {
			if (dataUser == null && !Loaded) {
				Loaded = true;
				/*if (SaveLoadHelper.LoadFromFile<UserInfos> (out dataUser)) {
					
				} else {*/

				dataUser = Resources.Load <UserInfos> ("Data/UserInfo");

/*					UserInfos ui = new UserInfos ();
					ui.userData = new UserInfos.UserData[Database.Get.playersData.Length];

					for (int i = 0; i < ui.userData.Length; i++) {
						ui.userData [i] = new UserInfos.UserData ();
					}*/

				//SaveLoadHelper.SaveToFile<UserInfos> (dataUser);
				//}
			}

			return dataUser;
		}
	}

	public static void SetPlayerIndex (int index)
	{
		GetInfo.curPlayerIndex = index;
	}

	public static bool BuyWithCoin (int coinAmount)
	{
		if (Coins - coinAmount >= 0) {
			Coins -= coinAmount;
			return true;
		} else {
			
			return false;
		}
	}

}