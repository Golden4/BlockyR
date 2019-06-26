using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpecialItem : SpecialItem {

	Vector2I coinCount = new Vector2I (1, 3);

	public override void PickUp (Player player)
	{
		int coinAmount = Random.Range (coinCount.x, coinCount.y);

		User.AddCoin (coinAmount);

		Vector3 fromPos = Camera.main.WorldToScreenPoint (transform.position);
		Vector3 toPos = CoinUI.Ins.coinImage.transform.position;
		Utility.CoinsAnimate (CoinUI.Ins, CoinUI.Ins.coinImage.gameObject, CoinUI.Ins.transform, coinAmount / 3 + 1, fromPos, toPos, .5f, CoinUI.Ins.curve, () => {
			AudioManager.PlaySoundFromLibrary ("Coin");
		});

	}
}
