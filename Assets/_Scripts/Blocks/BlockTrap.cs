using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrap : BlockGrass {
	
	public GameObject obj;
	static GameObject[] objPrefabs;

	static string[] pathToObj = {
		"Models/Shipi/Shipi"
	};

	public override void OnGenerateBlockMesh ()
	{
		GameObject[] biomeObjects = objPrefabs;
		this.obj = MonoBehaviour.Instantiate (biomeObjects [Random.Range (0, biomeObjects.Length)]);
		obj.transform.SetParent (chunk.transform, false);
		obj.transform.position = new Vector3 (worldCoords.x, .35f, worldCoords.y);
		obj.isStatic = true;
	}

	public override bool CanDie ()
	{
		return true;
	}

	public override DieInfo dieInfo ()
	{
		return DieInfo.Trap;
	}

	public BlockTrap (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
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
