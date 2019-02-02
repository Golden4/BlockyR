using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectsManager : MonoBehaviour {
	public static MovingObjectsManager Ins;

	public Balk balkPrefab;

	public List<int> linesToGenerate = new List<int> ();

	public Dictionary<int, BalksLine> balks = new Dictionary<int, BalksLine> ();

	public LayerMask balkMask;

	void Awake ()
	{
		Ins = this;

		Chunk.OnGenerateChunk += OnGenerateChunk;
	}

	public static void AddLineToGenerateBalks (int line)
	{
		if (!Ins.linesToGenerate.Contains (line))
			Ins.linesToGenerate.Add (line);
	}

	void OnDestroy ()
	{
		Chunk.OnGenerateChunk -= OnGenerateChunk;
	}

	void OnGenerateChunk (Vector2I coords)
	{
		int chunkCoord = coords.x * Chunk.size;

		for (int i = 0; i < Chunk.size; i++) {
			int line = chunkCoord + i;
			if (linesToGenerate.Contains (line)) {
				if (!balks.ContainsKey (line)) {
					if (World.Ins.outChunkIndexMinX < line) {
						GenerateBalkLine (line);
					}
					//linesToGenerate.Remove (line);
				}
			}
		}
		
		/*if (balks.ContainsKey (coords.y))
		if (balks [coords.y] == null) {
			GenerateBalkLine (coords.y);
		}*/

	}

	void GenerateBalkLine (int line)
	{
//		print (line);
		//1.1 0.8 0.5 0.2
		BalksLine bl = SpawnBalksInLine (line, 5, 9, (Balk.BalkDirection)(line % 2), Random.Range (1.9f, 2.5f), 0.2f + Random.Range (0, 4) * 0.3f);
		balks [line] = bl;
	}

	BalksLine SpawnBalksInLine (int line, int count, float spaceBetweenBlocks, Balk.BalkDirection dir, float speed, float size)
	{
		Balk[] balksList = new Balk[count];

		int finalIndexY = World.Ins.outChunkIndexMaxY;

		for (int y = 0; y < count; y++) {

			Balk curBalk = SpawnBalkInLine (line, finalIndexY * Chunk.size - y * spaceBetweenBlocks, dir, speed, size);

			/*	int num = (dir == Balk.BalkDirection.Left) ? 0 : count - 1;

			if (y == num) {
				lastBalk = curBalk;
			}*/

			balksList [y] = curBalk;

		}

		return new BalksLine (balksList, dir);
	}

	Balk SpawnBalkInLine (int line, float yCoord, Balk.BalkDirection dir, float speed, float size)
	{
		Balk balk =	Instantiate (balkPrefab.gameObject).GetComponent <Balk> ();

		balk.line = line;

		balk.transform.SetParent (transform, false);

		balk.transform.position = new Vector3 (line, .1f, yCoord);
		balk.transform.localScale = new Vector3 (balk.transform.localScale.x, balk.transform.localScale.y, size);
		balk.dir = dir;
		balk.speed = speed;
		return balk;

	}

	public void DestroyBalksInLine (int line)
	{
		foreach (Balk balk in balks[line].balks) {
			Destroy (balk.gameObject);
		}

		balks.Remove (line);

	}

	public static Vector3 GetStartBalkPosition (int line)
	{

		List<Balk> gos = new List<Balk> (Ins.balks [line].balks);

		float[] zPos = new float[gos.Count];

		for (int i = 0; i < gos.Count; i++) {
			zPos [i] = gos [i].transform.position.z;
		}

		Balk lastBalk = null;

		float value = (Ins.balks [line].balksDirection == Balk.BalkDirection.Right) ? Mathf.Min (zPos) : Mathf.Max (zPos);

		int index = -1;

		for (int i = 0; i < zPos.Length; i++) {
			if (zPos [i] == value) {
				index = i;
			}
		}

		lastBalk = gos [index];

		return new Vector3 (line, .1f, lastBalk.transform.position.z - 8 * ((int)Ins.balks [line].balksDirection * 2 - 1));
	}

	[System.Serializable]
	public class BalksLine {
		public Balk[] balks;
		public Balk.BalkDirection balksDirection;

		public BalksLine (Balk[] balks, Balk.BalkDirection balksDirection)
		{
			this.balks = balks;
			this.balksDirection = balksDirection;
		}
	}
}
