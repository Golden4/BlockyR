using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbility {

	public Action ablityAction;
	public Action onAblityEndAction;
	private float abilityTime;
	private float lastUseAbilityTime = -1;
	public bool isUsingAbility;

	public void Use ()
	{
		if (ablityAction != null) {
			ablityAction ();
		}

		lastUseAbilityTime = Time.time;

		isUsingAbility = true;

		Debug.Log ("AbilityUsing " + lastUseAbilityTime);

	}

	public void Update ()
	{
		if (isUsingAbility)
			if (lastUseAbilityTime + abilityTime < Time.time) {
				OnAbilityEnd ();
			}
	}

	public void OnAbilityEnd ()
	{
		if (onAblityEndAction != null) {
			onAblityEndAction ();
		}

		isUsingAbility = false;

		Debug.Log ("AbilityEnd " + Time.time);

	}

	public PlayerAbility (Action ablityAction, Action onAblityEndAction, float abilityTime)
	{
		this.ablityAction = ablityAction;
		this.onAblityEndAction = onAblityEndAction;
		this.abilityTime = abilityTime;
	}
	

}
