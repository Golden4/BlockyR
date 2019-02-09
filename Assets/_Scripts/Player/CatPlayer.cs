using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayer : Player {
	
	protected int lifeCount = 2;

	protected override void Start ()
	{
		base.Start ();
		retry = true;
	}

	public override void Die (DieInfo dieInfo)
	{
		base.Die (dieInfo);
		lifeCount--;

		if (lifeCount <= 0) {
			retry = false;
		}
	}

}
