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

	public string GetBiomesListString (int charInx)
	{
		string biomesStr = "";
		for (int i = 0; i < Database.Get.playersData [charInx].biomesList.Length; i++) {
			biomesStr += LocalizationManager.GetLocalizedText (BiomeController.Ins.biomesList [Database.Get.playersData [charInx].biomesList [i]].biomeID);
			if (i < Database.Get.playersData [charInx].biomesList.Length - 1)
				biomesStr += ", ";
		}
		return biomesStr;
	}

	[System.Serializable]
	public class BiomeInfo {
		public string biomeID;
		public GameObject[] obstacles;
	}
}

public enum Biome
{
	Forest = 0,
	Desert = 1,
	Snowy = 2,
	DarkForest = 3
}
