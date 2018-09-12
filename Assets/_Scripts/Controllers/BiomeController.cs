using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeController : MonoBehaviour {

	public static BiomeController Ins;

	public BiomeInfo[] biomesList;

	/*	public BlockComp GetBiomeBlock (int biomeIndex)
	{
		return biomesList [biomeIndex].blocks.comp;
	}*/

	void Awake ()
	{
		Ins = this;

	}

	[System.Serializable]
	public class BiomeInfo {
		public GameObject[] obstacles;
	}
}

public enum Biome
{
	Forest = 0,
	Desert = 1,
	Snowy = 2
}
