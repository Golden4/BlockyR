using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObstacle : BlockGrass {

	public GameObject obj;
	/*	static GameObject[] objPrefabsSummer;
	static GameObject[] objPrefabsWinter;
	static GameObject[] objPrefabsDesert;

	static string[] pathToObjsSummer = new string[] {
		"Models/Tree1/Tree1", "Models/Tree1/Tree2", "Models/Stone/Stone1"
	};

	static string[] pathToObjsWinter = new string[] {
		"Models/WinterTree/WinterTree1", "Models/WinterStone/WinterStone1"
	};

	static string[] pathToObjsDesert = new string[] {
		"Models/Cactus/Cactus", "Models/Cactus/Cactus1"
	};*/

	public override void OnGenerateBlockMesh ()
	{
		GameObject[] biomeObjects = BiomeController.Ins.biomesList [(int)biome].obstacles;

		this.obj = MonoBehaviour.Instantiate (biomeObjects [Random.Range (0, biomeObjects.Length)]);
		obj.transform.SetParent (chunk.transform, false);
		obj.transform.rotation = Quaternion.Euler (0, 90 * Random.Range (0, 4), 0);
		obj.transform.position = new Vector3 (worldCoords.x, .5f, worldCoords.y);
		obj.isStatic = true;
		//obj.transform.localScale = Vector3.one * Random.Range (.7f, 1f);
	}

	public override bool isWalkable ()
	{
		return false;
	}

	public BlockObstacle (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
/*		if (objPrefabsSummer == null || objPrefabsWinter == null) {
			objPrefabsSummer = new GameObject[pathToObjsSummer.Length];
			objPrefabsWinter = new GameObject[pathToObjsWinter.Length];
			objPrefabsDesert = new GameObject[pathToObjsDesert.Length];
		}

		for (int i = 0; i < pathToObjsSummer.Length; i++) {
			if (objPrefabsSummer [i] == null) {
				objPrefabsSummer [i] = (GameObject)Resources.Load (pathToObjsSummer [i]);
			}
		}

		for (int i = 0; i < pathToObjsWinter.Length; i++) {
			if (objPrefabsWinter [i] == null) {
				objPrefabsWinter [i] = (GameObject)Resources.Load (pathToObjsWinter [i]);
			}
		}

		for (int i = 0; i < pathToObjsDesert.Length; i++) {
			if (objPrefabsDesert [i] == null) {
				objPrefabsDesert [i] = (GameObject)Resources.Load (pathToObjsDesert [i]);
			}
		}
*/

	}


}
