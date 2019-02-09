using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitPlayer : Player {

	protected override void Start ()
	{
		base.Start ();
		jumpHeight = 0.9f;
	}

}
