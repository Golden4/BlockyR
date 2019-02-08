using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelImporter;

public class BlockTrap : BlockGrass {
	
	//public GameObject obj;
	//static GameObject[] objPrefabs = new GameObject[]{ GameAssets.i.pfShipi, GameAssets.i.pfBearTrap };

	//int curObjIndex = -1;

	public override void OnGenerateBlockMesh ()
	{
		GameAssets.GameAsset[] biomeObjects = new GameAssets.GameAsset[]{ GameAssets.i.asShipi, GameAssets.i.asBearTrap };

		data.curObjIndex = Random.Range (0, biomeObjects.Length);

		Vector3 pos = new Vector3 (worldCoords.x, .51f, worldCoords.y);
		Vector3 rot = new Vector3 (0, 90 * Random.Range (0, 4), 0);

		data.AddObject (biomeObjects [data.curObjIndex].CreateOnWorld (chunk.transform, pos, rot));
		/*this.obj = MonoBehaviour.Instantiate (biomeObjects [curObjIndex]);
		obj.transform.SetParent (chunk.transform, false);
		obj.transform.rotation = Quaternion.Euler (0, 90 * Random.Range (0, 4), 0);
		obj.transform.position = new Vector3 (worldCoords.x, .51f, worldCoords.y);
		obj.isStatic = true;*/
	}

	public override bool CanDie ()
	{
		return true;
	}

	public BlockTrap (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
		
	}

	public override void OnPlayerContact ()
	{
		base.OnPlayerContact ();

		if (data.curObjIndex == 1) {
			AudioManager.PlaySoundFromLibrary ("BearTrap");
		}

		VoxelFrameAnimationObject anim = data.GetObject ().pf.GetComponent <VoxelFrameAnimationObject> ();

		if (anim != null) {
			anim.ChangeFrame (1);
		}
	}
	

}
