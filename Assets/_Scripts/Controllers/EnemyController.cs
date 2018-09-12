using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public static EnemyController Ins;
	public EnemyPreset[] enemyPresets;

	public List<Enemy> spawnedEnemies = new List<Enemy> ();

	public List<Vector2I> blocksToSpawnEnemy = new List<Vector2I> ();

	void Awake ()
	{
		Ins = this;
	}

	IEnumerator Start ()
	{
		while (!Game.isGameStarted) {
			yield return null;
		}

		StartCoroutine (CheckAndSpawnEnemy ());
	}

	IEnumerator CheckAndSpawnEnemy ()
	{
		while (true) {

			float spawnTime = Random.Range (1, 5);

			yield return new WaitForSeconds (spawnTime);

			int enemyIndex = -1;
			Vector2I coords = null;

			Enemy enemy = GetEnemyPresetFromBiome ((Biome)GetBlockWalkable (out coords).biome, out enemyIndex);

			SpawnEnemyAtBlock (enemy, coords);
			print (spawnedEnemies.Count);

		}
	}

	public void AddBlockToSpawnEnemy (Vector2I worldCoords)
	{
		blocksToSpawnEnemy.Add (worldCoords);
	}

	public void RemoveBlockToSpawnEnemy (Vector2I worldCoords)
	{
		blocksToSpawnEnemy.Remove (worldCoords);
	}

	Enemy GetEnemyPresetFromBiome (Biome biome, out int enemyIndex)
	{
		enemyIndex = Random.Range (0, enemyPresets [(int)biome].prefabs.Length);

		return enemyPresets [(int)biome].prefabs [enemyIndex];
	}

	Block GetBlockWalkable (out Vector2I coords)
	{
		coords = FindRandomCoords ();

		Block block = World.Ins.GetBlock (coords);

		int i = 0;

		while (!block.CanDie () && !block.isWalkable () && i < 100) {
			coords = FindRandomCoords ();
			block = World.Ins.GetBlock (coords);
			i++;
		}

		return block;
	}

	public virtual Vector2I FindRandomCoords ()
	{
		Vector2I coords = Player.Ins.GetCurCoords ();

		coords.x += Random.Range (3, 7);
		coords.y += Random.Range (-2, 2);

		return coords;
	}

	public void SpawnEnemyAtBlock (Enemy enemy, Vector2I blockPos)
	{
		GameObject go = Instantiate (enemy.gameObject);
		go.transform.position = blockPos.ToVector3 (.5f);

		spawnedEnemies.Add (go.GetComponent <Enemy> ());

	}

	public void RemoveEnemy (Enemy enemy)
	{
		spawnedEnemies.Remove (enemy);
	}

}

[System.Serializable]
public class EnemyPreset {
	public Biome biome;
	public Enemy[] prefabs;
}