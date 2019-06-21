using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySpecialItem : SpecialItem {

	public override void PickUp (Player player)
	{
		if (player.ability != null)
			player.ability.PickUp ();
	}

}
