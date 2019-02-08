using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbility {

	public Action ablityAction;
	public Action onAblityEndAction;
	float abilityTime;
	float lastUseAbilityTime;


	public void Use ()
	{
		if (ablityAction != null) {
			ablityAction ();
		}

		lastUseAbilityTime = Time.time;
	}

	public void Update ()
	{
		
	}

	public void OnAbilityEnd ()
	{
		if (onAblityEndAction != null) {
			onAblityEndAction ();
		}
	}

	public PlayerAbility (Action ablityAction, Action onAblityEndAction, float abilityTime)
	{
		this.ablityAction = ablityAction;
		this.onAblityEndAction = onAblityEndAction;
		this.abilityTime = abilityTime;
	}
	

}
