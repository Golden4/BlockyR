using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuyCoinScreen : ScreenBase {

	public BuyCoinItem[] BuyCoinBtns;

	public ButtonIcon buyCoinScreenBtn;

	public override void OnActivate ()
	{
		base.OnActivate ();

		for (int i = 0; i < BuyCoinBtns.Length; i++) {
			BuyCoinBtns [i].priceText.text = PurchaseManager.Ins.GetLocalizedPrice (BuyCoinBtns [i].productID).ToString ();
		}

	}

	public void ShowBuyCoinScreen ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.BuyCoin);
	}

	public void BuyItem (string id)
	{
		PurchaseManager.Ins.BuyConsumable (id);
	}

	[System.Serializable]
	public class BuyCoinItem {
		public Button btn;
		public string productID;
		public Text priceText;
	}

	public Button getCoinsBtn;

	public Text timer;

	public TimeSpan nextGiftTime = new TimeSpan (0, 5, 0);

	public DateTime nextGiveGiftTime;

	public override void Init ()
	{
		base.Init ();
		if (PlayerPrefs.HasKey ("giftTime"))
			nextGiveGiftTime = new DateTime (long.Parse (PlayerPrefs.GetString ("giftTime")));
		getCoinsBtn.onClick.AddListener (GiveGift);

		if (CanTakeGift ()) {
			OnCanTakeGift ();
		} else {
			OnDontTakeGift ();
		}
	}

	void GiveGift ()
	{
		if (CanTakeGift ()) {

			nextGiveGiftTime = DateTime.Now.Add (nextGiftTime);

			PlayerPrefs.SetString ("giftTime", nextGiveGiftTime.Ticks.ToString ());

			int coinAmount = 10;
			User.AddCoin (coinAmount);

			Vector3 fromPos = getCoinsBtn.transform.position;
			Vector3 toPos = CoinUI.Ins.coinImage.transform.position;

			Utility.CoinsAnimate (CoinUI.Ins, CoinUI.Ins.coinImage.gameObject, CoinUI.Ins.transform, coinAmount, fromPos, toPos, .5f, CoinUI.Ins.curve, () => {
				AudioManager.PlaySoundFromLibrary ("Coin");
			});

		}
	}

	bool CanTakeGift ()
	{
		return nextGiveGiftTime.Ticks < DateTime.Now.Ticks;
	}

	bool lastOnTakeGiftBool;

	void Update ()
	{
		if ((nextGiveGiftTime - DateTime.Now).Ticks > 0) {

			if (!lastOnTakeGiftBool) {
				lastOnTakeGiftBool = true;
				OnDontTakeGift ();
			}

			TimeSpan ts = new DateTime ((nextGiveGiftTime - DateTime.Now).Ticks).TimeOfDay;

			timer.text = string.Format ("{0}", ts).Split ('.') [0];
		} else if (lastOnTakeGiftBool) {
			lastOnTakeGiftBool = false;
			OnCanTakeGift ();
		}
	}

	void OnCanTakeGift ()
	{
		buyCoinScreenBtn.changingColor = true;

		print ("OnCanTakeGift");
		getCoinsBtn.enabled = true;
		timer.gameObject.SetActive (false);
	}

	void OnDontTakeGift ()
	{
		buyCoinScreenBtn.changingColor = false;
		print ("OnDontTakeGift");
		getCoinsBtn.enabled = false;
		timer.gameObject.SetActive (true);
	}


}
