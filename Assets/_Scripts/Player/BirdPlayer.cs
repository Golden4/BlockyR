using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlayer : Player {

	private bool isFlying = false;

	protected override void Start ()
	{
		base.Start ();

		ability = new PlayerAbility (Fly, Walk, 2);
	}

	void Fly ()
	{
		isFlying = true;
		Debug.Log ("Flying!");
	}

	void Walk ()
	{
		isFlying = false;
		Debug.Log ("Walking!");
	}

}
