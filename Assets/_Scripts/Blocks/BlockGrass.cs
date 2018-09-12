using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrass : Block {
	
	public GameObject coin;
	static string coinPath = "Models/Coin/Coin";
	static ParticleSystem ps;
	static GameObject coinPrefab;

	public override Vector2I textureCoords (int dir)
	{
		if (dir < 4)
			return new Vector2I (1, (int)biome);
		else
			return new Vector2I (0, (int)biome);
	}

	public override void OnGenerateBlockMesh ()
	{
		if (Random.Range (0, 400) == 0) {
			coin = MonoBehaviour.Instantiate (coinPrefab);
			coin.transform.SetParent (chunk.transform, false);
			coin.transform.position = new Vector3 (worldCoords.x, .5f, worldCoords.y);
		}
	}

	public BlockGrass (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
		if (Random.Range (0, 20) == 0) {
			addedToSpawnEnemy = true;
			EnemyController.Ins.AddBlockToSpawnEnemy (worldCoords);
		}

		if (coinPrefab == null) {
			coinPrefab = Resources.Load <GameObject> (coinPath);
		}

		if (ps == null) {
			ps = MonoBehaviour.Instantiate<GameObject> (Resources.Load <GameObject> ("Particles/Grass")).GetComponent <ParticleSystem> ();
		}

	}

	bool addedToSpawnEnemy;

	public override void OnPlayerContact ()
	{
		if (biome == Biome.Forest) {
			ps.transform.position = Player.Ins.transform.position;
			ps.Play ();
		}
	}

	public override void OnBlockDestroy ()
	{
		if (addedToSpawnEnemy)
			EnemyController.Ins.RemoveBlockToSpawnEnemy (worldCoords);
	}
}
