using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour {
	public Saw sawPrefab;

	public List<SawLine> sawLines = new List<SawLine> ();

	void Start ()
	{
		Chunk.OnGenerateChunk += GenerateSaw;
	}

	void OnDestroy ()
	{
		Chunk.OnGenerateChunk -= GenerateSaw;
	}


	void GenerateSaw (Vector2I chunkCoords)
	{

		if (chunkCoords.x < 3)
			return;

		int chunkStartPointX = chunkCoords.x * Chunk.size;
		int chunkStartPointY = chunkCoords.y * Chunk.size;
		for (int i = 0; i < Chunk.size; i++) {
			if (Random.Range (0, 10) == 0) {
				int startPoint = 0;
				int endPoint = 0;
				if (FindStartAndEndPoints (chunkStartPointX + i, chunkStartPointY, ref startPoint, ref endPoint)) {

					Saw saw = Instantiate (sawPrefab).GetComponent <Saw> ();
					saw.transform.SetParent (transform, false);

					Vector3 startPointToMove = new Vector3 (chunkStartPointX + i, 0.5f, startPoint);
					Vector3 endPointToMove = new Vector3 (chunkStartPointX + i, 0.5f, endPoint);

					saw.transform.position = startPointToMove;

					saw.Init (startPointToMove, endPointToMove, chunkCoords);

					//SawLine line = new SawLine (saw, );
				}

			}
		}
	}

	bool FindStartAndEndPoints (int line, int startChunkPoint, ref int startPoint, ref int endPoint)
	{

		int correctLineCount = 0;
		//print (startChunkPoint + "   " + line);
		startPoint = startChunkPoint;
		for (int i = 0; i < Chunk.size; i++) {
			if (World.Ins.GetBlock (new Vector2I (line, i + startChunkPoint)).GetType () == typeof(BlockGrass)) {
				correctLineCount++;
			} else {
				correctLineCount = 0;
				startPoint = i + 1 + startChunkPoint;
			}

			if (correctLineCount > 5) {
				endPoint = startPoint + correctLineCount;
				return true;
			}

		}

		return false;

	}

	public class SawLine {

		public Saw saw;
		public int startCoord;
		public int endCoords;

		public SawLine (Saw saw, int startCoord, int endCoords)
		{
			this.saw = saw;
			this.startCoord = startCoord;
			this.endCoords = endCoords;
		}
	}

}
