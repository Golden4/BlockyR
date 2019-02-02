using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : ScreenBase {

	public GameObject itemsHolder;

	public ScrollSnap scrollSnap;

	public int curActiveItem {
		get {
			return scrollSnap.GetCurItemIndex;
		}
	}

	public int ItemCount {
		get {
			return scrollSnap.items.Length;
		}
	}

	public Text itemNameText;

	public Button SelectAndPlayBtn;

	public Button BuyBtn;

	public override void Init ()
	{
		scrollSnap.Init ();

		scrollSnap.OnChangeItemEvent += OnChangeItem;

		SelectAndPlayBtn.onClick.AddListener (() => {
			SelectAndPlay (curActiveItem);
			SceneController.RestartLevel ();
		});

		BuyBtn.onClick.AddListener (() => {
			BuyItem (curActiveItem);
		});

		for (int i = 0; i < ItemCount; i++) {
			scrollSnap.SetItemState (i, User.GetInfo.userData [i].bought);
		}
	}

	public override void OnActivate ()
	{
		UpdateItemState (curActiveItem);
		scrollSnap.SnapToObj (User.GetInfo.curPlayerIndex, false);
	}

	public override void OnDeactivate ()
	{
		base.OnDeactivate ();
	}

	public override void OnCleanUp ()
	{
		scrollSnap.OnChangeItemEvent -= OnChangeItem;
	}

	void OnChangeItem (int index)
	{
		itemNameText.text = Database.Get.playersData [index].name;
		UpdateItemState (index);
	}

	public void SelectAndPlay (int index)
	{
		User.SetPlayerIndex (index);
	}

	public void BuyItem (int index)
	{
		if (User.BuyWithCoin (Database.Get.playersData [index].price)) {
			User.GetInfo.userData [index].bought = true;
			UpdateItemState (index);
			scrollSnap.SetItemState (index, User.GetInfo.userData [index].bought);
			User.SaveUserInfoToFile ();
		}
	}

	public void UpdateItemState (int index)
	{
		bool bought = User.GetInfo.userData [index].bought;

		SelectAndPlayBtn.gameObject.SetActive (bought);
		BuyBtn.gameObject.SetActive (!bought);
		BuyBtn.GetComponentInChildren<Text> ().text = Database.Get.playersData [index].price.ToString ();
	}

	public void BackBtn ()
	{
		ScreenController.Ins.ActivateScreen (ScreenController.GameScreen.Menu);
	}

}
