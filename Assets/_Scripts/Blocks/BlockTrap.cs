using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelImporter;

public class BlockTrap : BlockGrass {
	
	public GameObject obj;
	static GameObject[] objPrefabs;
	int curObjIndex = -1;

	static string[] pathToObj = {
		"Models/Shipi/Shipi",
		"Models/BearTrap/BearTrap"
	};

	public override void OnGenerateBlockMesh ()
	{
		GameObject[] biomeObjects = objPrefabs;

		curObjIndex = Random.Range (0, biomeObjects.Length);

		this.obj = MonoBehaviour.Instantiate (biomeObjects [curObjIndex]);
		obj.transform.SetParent (chunk.transform, false);
		obj.transform.rotation = Quaternion.Euler (0, 90 * Random.Range (0, 4), 0);
		obj.transform.position = new Vector3 (worldCoords.x, .51f, worldCoords.y);
		obj.isStatic = true;
	}

	public override bool CanDie ()
	{
		return true;
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

	public override void OnPlayerContact ()
	{
		base.OnPlayerContact ();

		if (curObjIndex == 1) {
			AudioManager.PlaySoundFromLibrary ("BearTrap");
		}

		VoxelFrameAnimationObject anim = obj.GetComponent <VoxelFrameAnimationObject> ();

		if (anim != null) {
			anim.ChangeFrame (1);
		}
	}
	

}
