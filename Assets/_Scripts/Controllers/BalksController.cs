using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalksController : MonoBehaviour {
	public static BalksController Ins;

	public Balk[] balkPrefabs;

	public List<int> linesToGenerate = new List<int> ();

	public Dictionary<int, BalksLine> balksLine = new Dictionary<int, BalksLine> ();

	public LayerMask balkMask;

	void Awake ()
	{
		Ins = this;

		Chunk.OnGenerateChunk += OnGenerateChunk;
		Chunk.OnDestroyChunk += OnDestroyChunk;
	}

	public static void AddLineToGenerateBalks (int line)
	{
		if (!Ins.linesToGenerate.Contains (line))
			Ins.linesToGenerate.Add (line);
	}

	void OnDestroy ()
	{
		Chunk.OnGenerateChunk -= OnGenerateChunk;
		Chunk.OnDestroyChunk -= OnDestroyChunk;
	}

	void OnGenerateChunk (Vector2I coords)
	{
		int chunkCoord = coords.x * Chunk.size;

		for (int i = 0; i < Chunk.size; i++) {
			int line = chunkCoord + i;
			if (linesToGenerate.Contains (line)) {
				if (!balksLine.ContainsKey (line)) {
					if (World.Ins.outChunkIndexMinX < line) {
						GenerateBalkLine (line);
					}
					linesToGenerate.Remove (line);
				}
			}
		}
		
		/*if (balks.ContainsKey (coords.y))
		if (balks [coords.y] == null) {
			GenerateBalkLine (coords.y);
		}*/
	}

	void OnDestroyChunk (Vector2I coords)
	{
		
		int chunkCoord = coords.x * Chunk.size;

		for (int i = 0; i < Chunk.size; i++) {
			int line = chunkCoord + i;

			if (balksLine.ContainsKey (line)) {
				print (coords + "  " + World.Ins.outChunkIndexMaxX + "  " + World.Ins.outChunkIndexMinX + "   " + World.Ins.outChunkIndexMaxY + "   " + World.Ins.outChunkIndexMinY + "  " + line);
				if (coords.x < World.Ins.outChunkIndexMinX)
					DestroyBalksInLine (line);

			}
			
		}
	}

	void GenerateBalkLine (int line)
	{
//		print (line);
		//1.1 0.8 0.5 0.2

		int count = 5;
		float spaceBetweenBalks = Random.Range (7f, 10f);
		float speed = Mathf.Clamp (1.5f + UIScreen.Ins.score / 50f, 1.5f, 5f);
//		Debug.Log (speed + "  " + UIScreen.Ins.score);

		//BalksLine bl = SpawnBalksInLine (line, count, spaceBetweenBalks, (Balk.BalkDirection)(line % 2), speed);
		BalksLine bl = new BalksLine ((Balk.BalkDirection)(line % 2), line, speed, count, spaceBetweenBalks, this);
		balksLine [line] = bl;
	}

	/*BalksLine SpawnBalksInLine (int line, int count, float spaceBetweenBalks, Balk.BalkDirection dir, float speed)
	{
/*		Balk[] balksList = new Balk[count];

		int finalIndexY = World.Ins.outChunkIndexMaxY;

		for (int y = 0; y < count; y++) {

			Balk curBalk = SpawnBalkInLine (line, finalIndexY * Chunk.size - y * spaceBetweenBlocks, dir, speed, size);

			//int num = (dir == Balk.BalkDirection.Left) ? 0 : count - 1;

			//if (y == num) {
			//	lastBalk = curBalk;
			//}

			balksList [y] = curBalk;
		}

		return new BalksLine (dir, line, speed, count, spaceBetweenBalks, this);
	}*/

	/*	Balk SpawnBalkInLine (int line, float yCoord, Balk.BalkDirection dir, float speed, float size)
	{
		Balk balk =	Instantiate (balkPrefab.gameObject).GetComponent <Balk> ();

		balk.line = line;

		balk.transform.SetParent (transform, false);

		balk.transform.position = new Vector3 (line, .1f, yCoord);
		balk.transform.localScale = new Vector3 (balk.transform.localScale.x, balk.transform.localScale.y, size);
		balk.dir = dir;
		balk.speed = speed;
		return balk;

	}*/

	public void DestroyBalksInLine (int line)
	{
		print (line + " Destroyed");
		balksLine [line].DestroyAllBalks ();
		balksLine.Remove (line);
	}

	public static Vector3 GetStartBalkPosition (int line)
	{

		List<Balk> gos = new List<Balk> (Ins.balksLine [line].balks);

		float[] zPos = new float[gos.Count];

		for (int i = 0; i < gos.Count; i++) {

			if (!gos [i].isOnChunk) {
				zPos [i] = (gos [i].curBalkLine.dir == Balk.BalkDirection.Right) ? Mathf.Infinity : -Mathf.Infinity;
			} else {
				zPos [i] = gos [i].transform.position.z;
			}
		}

		Balk lastBalk = null;

		float value = (Ins.balksLine [line].dir == Balk.BalkDirection.Right) ? Mathf.Min (zPos) : Mathf.Max (zPos);

		int index = -1;

		for (int i = 0; i < zPos.Length; i++) {
			if (zPos [i] == value) {
				index = i;
			}
		}

		lastBalk = gos [index];

		return new Vector3 (line, .1f, lastBalk.transform.position.z - 8 * ((int)Ins.balksLine [line].dir * 2 - 1));
	}

	[System.Serializable]
	public class BalksLine {
		BalksController mom;
		public List<Balk> balks = new List<Balk> ();
		public Balk.BalkDirection dir;
		public int line;
		public float speed;
		float spaceBetweenBalks;

		public BalksLine (Balk.BalkDirection dir, int line, float speed, int count, float spaceBetweenBalks, BalksController mom)
		{
			this.dir = dir;
			this.mom = mom;
			this.speed = speed;
			this.line = line;
			this.spaceBetweenBalks = spaceBetweenBalks;
			SpawnBalks (count);
		}

		public void SpawnBalks (int count)
		{
			for (int i = 0; i < count; i++) {
				SpawnBalk ();
			}
		}

		public void DestroyAllBalks ()
		{
			for (int i = 0; i < balks.Count; i++) {
				Destroy (balks [i].gameObject);
			}
			balks.Clear ();
		}

		/*	public Vector3 GetNextSpawnBalkPos ()
		{
			Vector3 vec = new Vector3 ();
			vec.x = line;
			vec.y = 0.1f;

			if (balks [0] != null)
				vec.z = balks [0].transform.position.z - spaceBetweenBlocks * ((int)dir * 2 - 1);
			else
				vec.z = -8 * ((int)dir * 2 - 1);
			return vec;
		}*/

		public Balk SpawnBalk ()
		{

			int randomProbability = Random.Range (0, 10);
			int balkIndex;
			if (randomProbability < 2) {
				balkIndex = 0;
			} else if (randomProbability < 6) {
				balkIndex = 1;
			} else {
				balkIndex = 2;
			}

			Balk balk =	GameObject.Instantiate (mom.balkPrefabs [balkIndex].gameObject).GetComponent <Balk> ();

			balk.curBalkLine = this;

			balk.transform.SetParent (mom.transform, false);

			float yCoord = World.Ins.outChunkIndexMaxY * Chunk.size - balks.Count * spaceBetweenBalks;
			balk.size = Random.Range (0, 3);
			balk.transform.position = new Vector3 (line, .1f, yCoord);

			balks.Add (balk);
			balk.transform.name = mom.balkPrefabs [balkIndex].transform.name + "|Line:" + line + "|" + balks.Count;
			return balk;
		}

	}
}
