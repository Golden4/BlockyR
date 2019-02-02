using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInfo {

	public static DieInfo Water = new DieInfo ("Particles/WaterSplash", false, "WaterSplash");
	public static DieInfo Enemy = new DieInfo ("Particles/Die", true);
	public static DieInfo Fire = new DieInfo ("Particles/Die", true);
	public static DieInfo Trap = new DieInfo ("Particles/Die", true);

	public string pathToParticle;
	public string soundName;
	public bool hidePlayer;

	public DieInfo (string pathToParticle, bool hidePlayer, string soundName = "")
	{
		this.pathToParticle = pathToParticle;
		this.soundName = soundName;
		this.hidePlayer = hidePlayer;
	}
}
