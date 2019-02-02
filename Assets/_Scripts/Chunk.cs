using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour {

	public static int size = 6;

	public Vector2I chunkCoords{ get; set; }

	public Block[,] blocksInChunk = new Block[size, size];

	public static event System.Action<Vector2I> OnGenerateChunk;

	public int[,] biomesMap;
	public int[,] blocksMap;
	public int[,] objectsMap;

	public World world;

	public bool generated;

	Mesh mesh;
	MeshData meshData;
	MeshRenderer mr;

	public void GenerateChunkMesh ()
	{
		for (int x = 0; x < biomesMap.GetLength (0); x++) {
			for (int y = 0; y < biomesMap.GetLength (1); y++) {
				blocksInChunk [x, y].AddMeshData (meshData);
			}
		}

		generated = true;

		RenderMeshData (meshData);

		if (OnGenerateChunk != null)
			OnGenerateChunk (chunkCoords);

		//UnityEngine.Debug.Log (sw.ElapsedMilliseconds);
	}

	void GenerateMap ()
	{
		biomesMap = NoiseMap.BiomesMap (size, size, chunkCoords.x * size, chunkCoords.y * size);
		blocksMap = NoiseMap.BlocksMap (size, size, chunkCoords.x * size, chunkCoords.y * size, ref biomesMap);
		//objectsMap = NoiseMap.ObjectsMap (size, size, chunkCoords.y * size, chunkCoords.x * size);

		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				CreateBlock (x, y);
			}
		}

		int snowBlockCount = 0;

		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				if (biomesMap [x, y] == 2) {
					snowBlockCount++;
					if (snowBlockCount > 20) {
						GameObject go = (GameObject)Resources.Load ("Particles/Snow");
						GameObject snow = Instantiate (go);
						snow.transform.SetParent (transform, false);
						snow.transform.localPosition = new Vector3 (2.5f, 7, 2.5f);
						return;
					}
				}
			}
		}

	}

	void CreateBlock (int x, int y)
	{
		int biomeBlock = blocksMap [y, x];

		if (biomeBlock == 0) {
			blocksInChunk [x, y] = new BlockGrass (x, y, this, (Biome)biomesMap [x, y]);
		} else if (biomeBlock == 1/* && biomesMap [x, y] != 1*/) {
				blocksInChunk [x, y] = new BlockWater (x, y, this, (Biome)biomesMap [x, y]);
			} else {
				if ((Biome)Random.Range (0, 2) == Biome.Forest) {
					blocksInChunk [x, y] = new BlockTrap (x, y, this, (Biome)biomesMap [x, y]);
				} else
					blocksInChunk [x, y] = new BlockObstacle (x, y, this, (Biome)biomesMap [x, y]);
			}

	}


	public void OnCreateChunk ()
	{
		mesh = GetComponent <MeshFilter> ().mesh;
		mr = GetComponent <MeshRenderer> ();
		meshData = new MeshData ();
		transform.position = new Vector3 (chunkCoords.x * size, 0, chunkCoords.y * size);

		GenerateMap ();
	}

	public void DestroyChunk ()
	{
		for (int x = 0; x < blocksInChunk.GetLength (0); x++) {
			for (int y = 0; y < blocksInChunk.GetLength (1); y++) {
				blocksInChunk [x, y].OnBlockDestroy ();
			}
		}

		Destroy (gameObject);
	}

	static Material[] blockMat;

	public void RenderMeshData (MeshData data)
	{

		if (blockMat == null) {
			blockMat = new Material[3];
			blockMat [0] = Resources.Load <Material> ("Materials/BlocksMat");
			blockMat [1] = Resources.Load <Material> ("Materials/BlocksMat2");
			blockMat [2] = Resources.Load <Material> ("Materials/BlocksMat3");
		}

		mr.sharedMaterials = blockMat;

		mesh.Clear ();
		mesh.vertices = data.verticies.ToArray ();

		mesh.subMeshCount = 3;
		mesh.SetTriangles (data.triangles.ToArray (), 0);
		mesh.SetTriangles (data.triangles2.ToArray (), 1);
		mesh.SetTriangles (data.triangles3.ToArray (), 2);

		mesh.uv = data.uvs.ToArray ();
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		//gameObject.AddComponent <MeshCollider> ();
	}

	public Block GetBlockLocalCoords (Vector2I localCoords)
	{
		if (BlockInChunk (localCoords))
			return blocksInChunk [localCoords.x, localCoords.y];

		return world.GetBlock (localCoords + chunkCoords * size);
	}

	public bool BlockInChunk (Vector2I localCoords)
	{
		if (localCoords.x < 0 || localCoords.x >= size || localCoords.y < 0 || localCoords.y >= size) {
			return false;
		}

		return true;
	}

	public static Vector2I WorldToChunkCoord (Vector2I worldCoord)
	{
		float multiple = size;
		Vector2I pos = new Vector2I ();

		pos.x = Mathf.FloorToInt (worldCoord.x / multiple);
		pos.y = Mathf.FloorToInt (worldCoord.y / multiple);

		return pos;
	}

	public Vector2I WorldToLocalCoord (Vector2I worldCoords)
	{
		int x = worldCoords.x - chunkCoords.x * size;
		int y = worldCoords.y - chunkCoords.y * size;

		return new Vector2I (x, y);
	}
}