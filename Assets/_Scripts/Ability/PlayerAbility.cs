using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbility {
	
	public bool canUseAbility = true;

	public Action ablityAction;
	public Action onAblityEndAction;
	protected float abilityTime;
	protected float lastUseAbilityTime = -1;
	public bool isUsingAbility;

	public void Use ()
	{
		if (!canUseAbility)
			return;
		
		if (ablityAction != null) {
			ablityAction ();
		}

		lastUseAbilityTime = Time.time;

		isUsingAbility = true;

		canUseAbility = false;

		Debug.Log ("AbilityUsing " + lastUseAbilityTime);
	}

	public virtual void Update ()
	{
		if (isUsingAbility) {
			if (lastUseAbilityTime + abilityTime < Time.time) {
				OnAbilityEnd ();
			}
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

	public virtual void PickUp ()
	{
		canUseAbility = true;
	}

	public PlayerAbility (Action ablityAction, Action onAblityEndAction, float abilityTime)
	{
		this.ablityAction = ablityAction;
		this.onAblityEndAction = onAblityEndAction;
		this.abilityTime = abilityTime;
	}
	

}
