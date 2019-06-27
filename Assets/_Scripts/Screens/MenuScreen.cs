﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScreen : ScreenBase {

	public static event System.Action OnStartGame;

	public Button startGameBtn;

	public Button freeCoinsBtn;

	public Text gameTitleText;

	public static MenuScreen Ins;

	public override void Init ()
	{
		base.Init ();
		Ins = this;
	}

	void Start ()
	{
		SceneController.Init ();
		ShowGameTitle (true, true);
		//if (AdController.Ins.RewardedADLoaded ()) {
		freeCoinsBtn.gameObject.SetActive (true);
		freeCoinsBtn.onClick.RemoveAllListeners ();
		freeCoinsBtn.onClick.AddListener (GetFreeCoins);
		/*} else {
			freeCoinsBtn.gameObject.SetActive (false);
		}*/

		startGameBtn.onClick.RemoveAllListeners ();
		startGameBtn.onClick.AddListener (StartGame);

		freeCoinsBtn.gameObject.GetComponent<ButtonIcon> ().ActivateBtn ();

	}

	/*void Update ()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN

		if (Input.anyKeyDown && !EventSystem.current.IsPointerOverGameObject ()) {
			
			StartGame ();
		}
		#else
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
			if (!EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId))
				StartGame ();
		}

		#endif
	}*/

	public void ShowGameTitle (bool show, bool fade)
	{
		gameTitleText.enabled = true;

		if (fade) {
			if (show) {
				gameTitleText.GetComponent <GUIAnim> ().MoveIn ();
			} else {
				gameTitleText.GetComponent <GUIAnim> ().MoveOut ();
			}
		}

		if (!fade && !show) {
			gameTitleText.enabled = false;
			gameTitleText.GetComponent <GUIAnim> ().MoveOut ();
		}

	}

	void GetFreeCoins ()
	{
		if (AdController.Ins.RewardedADLoaded ()) {
			AdController.Ins.ShowRewardedAD ();
		}

		freeCoinsBtn.gameObject.GetComponent<ButtonIcon> ().DeactivateBtn ();


/*		int coinAmount = 10;
		User.AddCoin (coinAmount);

		Vector3 fromPos = MenuScreen.Ins.freeCoinsBtn.transform.position;
		Vector3 toPos = CoinUI.Ins.coinImage.transform.position;

		Utility.CoinsAnimate (CoinUI.Ins, CoinUI.Ins.coinImage.gameObject, CoinUI.Ins.transform, coinAmount, fromPos, toPos, .5f, CoinUI.Ins.curve, () => {
			AudioManager.PlaySoundFromLibrary ("Coin");
		});*/

	}

	public override void OnActivate ()
	{
		base.OnActivate ();
		ShowGameTitle (true, true);
	}

	public override void OnDeactivate ()
	{
		base.OnDeactivate ();

		ShowGameTitle (false, false);

	}

	public void StartGame ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.UI);

		ShowGameTitle (false, true);

		if (OnStartGame != null)
			OnStartGame.Invoke ();
	}

	public void ShowShopScreen ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.Shop);
	}
}
