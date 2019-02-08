using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWaterLily : BlockWater {
	//static GameObject[] objPrefabs = new GameObject[]{ GameAssets.i.pfWaterLily };

	public override void OnGenerateBlockMesh ()
	{
		Vector3 pos = new Vector3 (worldCoords.x, .2f, worldCoords.y);
		Vector3 rot = new Vector3 (0, Random.Range (0, 360), 0);


		data.AddObject (GameAssets.i.asWaterLily.CreateOnWorld (chunk.transform, pos, rot));

		/*this.obj = MonoBehaviour.Instantiate (biomeObjects [curObjIndex]);
		obj.transform.SetParent (chunk.transform, false);
		obj.transform.rotation = Quaternion.Euler (0, Random.Range (0, 360), 0);
		obj.transform.position = new Vector3 (worldCoords.x, .2f, worldCoords.y);
		obj.isStatic = true;*/
	}

	public BlockWaterLily (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
		
	}
	
	
}
