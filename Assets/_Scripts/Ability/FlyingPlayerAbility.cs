using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPlayerAbility : PlayerAbility {

	public float abilityAmount = 0;

	public override void Update ()
	{
		base.Update ();
		if (isUsingAbility) {
			abilityAmount = 1 - (Time.time - lastUseAbilityTime) / abilityTime;
			UIScreen.Ins.SetAbilityAmountUI (abilityAmount);
		}
	}

	public override void PickUp ()
	{
		base.PickUp ();

		UIScreen.Ins.SetAbilityAmountUI (1);
	}

	public FlyingPlayerAbility (System.Action ablityAction, System.Action onAblityEndAction, float abilityTime) : base (ablityAction, onAblityEndAction, abilityTime)
	{
	}
	

}
