using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWaterLily : BlockWater {
	public GameObject obj;
	static GameObject[] objPrefabs;
	int curObjIndex = -1;

	static string[] pathToObj = {
		"Models/WaterLily/WaterLily"
	};

	public override void OnGenerateBlockMesh ()
	{
		GameObject[] biomeObjects = objPrefabs;

		curObjIndex = Random.Range (0, biomeObjects.Length);

		this.obj = MonoBehaviour.Instantiate (biomeObjects [curObjIndex]);
		obj.transform.SetParent (chunk.transform, false);
		obj.transform.rotation = Quaternion.Euler (0, Random.Range (0, 360), 0);
		obj.transform.position = new Vector3 (worldCoords.x, .2f, worldCoords.y);
		obj.isStatic = true;
	}

	public BlockWaterLily (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
		if (objPrefabs == null) {
			objPrefabs = new GameObject[pathToObj.Length];
		}

		for (int i = 0; i < pathToObj.Length; i++) {
			if (objPrefabs [i] == null) {
				objPrefabs [i] = (GameObject)Resources.Load (pathToObj [i]);
			}
		}
	}
	
	
}
