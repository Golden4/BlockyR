using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrogPlayer : Player {

	protected override void OnPlayerStepOnBlock ()
	{
		if (!isSnaped) {

			curBlock.OnPlayerContact ();

			if (curBlock.CanDie ()) {
				Type blockType = curBlock.GetType ();


				if (blockType == typeof(BlockWaterLily)) {
					return;
				}

				DieInfo dieInfo;

				if (blockType.IsSubclassOf (typeof(BlockWater)) || blockType == typeof(BlockWater)) {
					dieInfo = DieInfo.Water;
				} else {
					dieInfo = DieInfo.Trap;
				}

				Die (dieInfo);
			}
		}

		if (isSnaped)
			AudioManager.PlaySoundFromLibrary ("WoodJump");
		else if (curBlock.biome == Biome.Snowy)
				AudioManager.PlaySoundFromLibrary ("SnowJump");
	}
}
