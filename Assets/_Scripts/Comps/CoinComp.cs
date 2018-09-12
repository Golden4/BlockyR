using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinComp : MonoBehaviour {

	public float rotationSpeed = 80;
	Vector2I coinCount = new Vector2I (1, 10);

	void Update ()
	{
		transform.localEulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
		Vector3 pos = transform.localPosition;
		pos.y = Mathf.Sin (Time.time * 1.5f) * 0.2f + 1;
		transform.localPosition = pos;
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			PickUp ();
			Destroy ();
		}
	}

	void PickUp ()
	{
		int coinAmount = Random.Range (coinCount.x, coinCount.y);

		User.AddCoin (coinAmount);

		Vector3 fromPos = Camera.main.WorldToScreenPoint (transform.position);
		Vector3 toPos = CoinUI.Ins.coinImage.transform.position;
		Utility.CoinsAnimate (CoinUI.Ins, CoinUI.Ins.coinImage.gameObject, CoinUI.Ins.transform, coinAmount / 3 + 1, fromPos, toPos, .5f, CoinUI.Ins.curve, () => {
			AudioManager.PlaySoundFromLibrary ("Coin");
		});

	}

	void Destroy ()
	{
		Destroy (gameObject);
	}
}
