using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInfo {

	public static DieInfo Water = new DieInfo ("Particles/WaterSplash", false);
	public static DieInfo Enemy = new DieInfo ("Particles/Die", true);
	public static DieInfo Fire = new DieInfo ("Particles/Die", true);
	public static DieInfo Trap = new DieInfo ("Particles/Die", true);

	public string pathToParticle;
	public string pathToSound;
	public bool hidePlayer;

	public DieInfo (string pathToParticle, bool hidePlayer, string pathToSound = "")
	{
		this.pathToParticle = pathToParticle;
		this.pathToSound = pathToSound;
		this.hidePlayer = hidePlayer;
	}
}
