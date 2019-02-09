using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
	public static World Ins;

	public Dictionary<Vector2I, Chunk> chunks = new Dictionary<Vector2I, Chunk> ();

	public int curActiveChunkIndex;

	Player player;

	public int textureSize = 2;

	void Awake ()
	{
		Ins = this;
	}

	void Start ()
	{
		player = Player.Ins;
		GenerateChunks ();

		StartCoroutine (UpdateChunksCoroutine (true));
		StartCoroutine (UpdateChunksCoroutine (false));
		OnChangeChunk ();
	}

	Vector2I oldChunkPos = new Vector2I (-10, -10);
	Vector2I curChunkPos = new Vector2I ();

	void Update ()
	{
		curChunkPos = WorldPositionToChunkCoords (player.transform.position);

		if (oldChunkPos != curChunkPos) {
			OnChangeChunk ();
			oldChunkPos = curChunkPos;
		}
	}

	public int outChunkIndexMinX {
		get {
			return -chunkAroundPlayerBackward + curChunkPos.x;
		}
	}

	public int outChunkIndexMinY {
		get {
			return -chunkAroundPlayerBackward + curChunkPos.y;
		}
	}

	public int outChunkIndexMaxX {
		get {
			return chunkAroundPlayerForward + curChunkPos.x;
		}
	}

	public int outChunkIndexMaxY {
		get {
			return chunkAroundPlayerForward + curChunkPos.y;
		}
	}

	void OnChangeChunk ()
	{

//		print (outChunkIndexMinX + "  " + outChunkIndexMaxX + "   " + outChunkIndexMinY + "   " + outChunkIndexMaxY + "   " + curChunkPos);

		GenerateChunks ();
	}

	public static Vector2I WorldPositionToChunkCoords (Vector3 worldPos)
	{
		Vector2I vec = new Vector2I ();
		vec.x = (int)System.Math.Round ((float)(worldPos.x - Chunk.size / 2) / Chunk.size);
		vec.y =	(int)System.Math.Round ((float)(worldPos.z - Chunk.size / 2) / Chunk.size);

		return vec;
	}

	int chunkAroundPlayerForward = 4;
	int chunkAroundPlayerBackward = 1;

	void GenerateChunks ()
	{
		for (int x = curChunkPos.x - chunkAroundPlayerBackward; x <= curChunkPos.x + chunkAroundPlayerForward; x++) {
			for (int y = curChunkPos.y - chunkAroundPlayerBackward; y <= curChunkPos.y + chunkAroundPlayerForward; y++) {
				
				if (GetChunkFromChunkCoords (new Vector2I (x, y)) == null) {
					CreateChunk (x, y);
				}
			}
		}
	}

	static GameObject chunkHolder;

	void CreateChunk (int x, int y)
	{

		if (chunkHolder == null) {
			chunkHolder = new GameObject ("Chunks");
		}

		Chunk chunk = new GameObject ("Chunk: " + x + " | " + y).AddComponent <Chunk> ();
		chunk.transform.SetParent (chunkHolder.transform, false);
		chunk.chunkCoords = new Vector2I (x, y);
		chunk.world = this;
		chunk.OnCreateChunk ();
		chunk.gameObject.isStatic = true;
		chunks [chunk.chunkCoords] = chunk;

	}

	IEnumerator UpdateChunksCoroutine (bool repeatOnce)
	{
		bool infinity = true;

		while (infinity) {
			
			List<Chunk> chunkTemp = new List<Chunk> (chunks.Values);

			foreach (Chunk chunk in chunkTemp) {
				if (!chunk.generated) {
					chunk.GenerateChunkMesh ();
					if (!repeatOnce)
						yield return null;
				}
			}

			foreach (Chunk chunk in chunkTemp) {
				if (chunk.chunkCoords.x - curChunkPos.x < -chunkAroundPlayerBackward || chunk.chunkCoords.y - curChunkPos.y < -chunkAroundPlayerBackward || chunk.chunkCoords.x - curChunkPos.x > chunkAroundPlayerForward || chunk.chunkCoords.y - curChunkPos.y > chunkAroundPlayerForward) {
					chunks.Remove (chunk.chunkCoords);
					chunk.DestroyChunk ();
					if (!repeatOnce)
						yield return null;
				}
			}

			if (!repeatOnce)
				yield return null;

			if (repeatOnce)
				infinity = false;

		}
	}

	public Chunk GetChunkFromChunkCoords (Vector2I coord)
	{
		Chunk chunk = null;
		chunks.TryGetValue (coord, out chunk);
		return chunk;
	}

	public Block GetBlock (Vector2I worldCoords)
	{
		Chunk chunk = GetChunkFromChunkCoords (Chunk.WorldToChunkCoord (worldCoords));

		if (chunk == null)
			return null;

		Block block = chunk.GetBlockLocalCoords (chunk.WorldToLocalCoord (worldCoords));

		return block;
	}

	public void SetBlock (Block block, int x, int y)
	{

		Vector2I worldCoords = new Vector2I (x, y);

		Chunk chunk = GetChunkFromChunkCoords (Chunk.WorldToChunkCoord (worldCoords));

		if (chunk == null)
			Debug.LogError ("Not Found Chunk: " + x + "  " + y);
		return;

		chunk.SetBlockLocalCoords (block, chunk.WorldToLocalCoord (worldCoords));

	}

	public static bool IsOnChunk (Vector3 position)
	{
		Vector2I curChunkCoord = World.WorldPositionToChunkCoords (position);

		if (World.Ins.outChunkIndexMinX - 2 < curChunkCoord.x && World.Ins.outChunkIndexMinY - 2 < curChunkCoord.y && World.Ins.outChunkIndexMaxX + 2 > curChunkCoord.x && World.Ins.outChunkIndexMaxY + 2 > curChunkCoord.y) {
			return true;
		}

		return false;
	}

	public static Vector2I FindSpawnPos (Vector2I coords)
	{
		Vector2I coordToSpawn = new Vector2I ();

		bool finded = false;

		for (int _x = 0; _x < 30; _x++) {
			Block block;
			for (int _y = 0; _y < 5; _y++) {
				for (int v = 0; v < 2; v++) {
					block = World.Ins.GetBlock (new Vector2I (coords.x - _x, coords.y + ((v == 0) ? 1 : -1) * _y));
					if (CorrectBlock (block)) {
						coordToSpawn = block.worldCoords;
						return coordToSpawn;
					}
				}
			}
		}

		for (int _x = 0; _x < 30; _x++) {
			Block block;
			for (int _y = 0; _y < 5; _y++) {
				for (int v = 0; v < 2; v++) {
					block = World.Ins.GetBlock (new Vector2I (coords.x + _x, coords.y + ((v == 0) ? 1 : -1) * _y));
					if (CorrectBlock (block)) {
						coordToSpawn = block.worldCoords;
						return coordToSpawn;
					}
				}
			}
		}


		Debug.LogError ("Not Found SpawnPos: " + coords);
		return coordToSpawn;
	}

	public static bool CorrectBlock (Block block)
	{
		return block != null && !block.CanDie () && block.isWalkable ();
	}

}
