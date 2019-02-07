using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block {

	public BlockData data;
	public Vector2I localChunkCoords;
	public Vector2I worldCoords;
	public Chunk chunk;
	public Biome biome;

	public virtual Vector2I textureCoords (int dir)
	{
		return new Vector2I (0, 0);
	}

	public virtual bool CanDie ()
	{
		return false;
	}

	public virtual void OnPlayerContact ()
	{
		
	}

	public virtual bool isWalkable ()
	{
		return true;
	}

	public MeshData AddMeshData (MeshData mesh)
	{
		MakeBlock (new Vector3 (localChunkCoords.x, 0, localChunkCoords.y), mesh);
		return mesh;
	}

	public virtual void MakeBlock (Vector3 pos, MeshData mesh)
	{
		for (int i = 0; i < 5; i++) {
			if (i < 4) {
				if (!GetNeighbor (localChunkCoords.x, localChunkCoords.y, (Direction)i))
					MakeFace (i, pos, mesh);
			} else {
				MakeFace (i, pos, mesh);
			}
		}

		OnGenerateBlockMesh ();

	}

	public virtual float GetBlockHeight ()
	{
		return 0;
	}

	public virtual void OnGenerateBlockMesh ()
	{
		
	}

	public virtual void MakeFace (int dir, Vector3 pos, MeshData mesh)
	{
		mesh.verticies.AddRange (CubeMeshData.faceVertices (dir, .5f, pos));

		int vCount = mesh.verticies.Count - 4;

		int[] trianglesList = CubeMeshData.faceTriangles [dir];

		int[] triangle = {
			vCount,
			vCount + 1,
			vCount + 2,
			vCount,
			vCount + 2,
			vCount + 3
		};

		if (Random.Range (0, 3) == 0) {
			mesh.triangles.AddRange (triangle);
		} else if (Random.Range (0, 3) == 0) {
				mesh.triangles2.AddRange (triangle);
			} else {
				mesh.triangles3.AddRange (triangle);
			}

		mesh.uvs.AddRange (CubeMeshData.faceUvs (this.textureCoords (dir).x, this.textureCoords (dir).y, 1f / World.Ins.textureSize));
	}

	public bool GetNeighbor (int x, int y, Direction dir)
	{
		Vector2I faceToCheck = CubeMeshData.offsets [(int)dir];
		Vector2I neiborCoord = new Vector2I (x + faceToCheck.x, y + faceToCheck.y);

		Block block = chunk.GetBlockLocalCoords (neiborCoord);

		if (block == null || block is BlockWater)
			return false;
		else
			return true;
	}

	public virtual void OnBlockDestroy ()
	{
		
	}

	public Block (int x, int y, Chunk chunk, Biome biome)
	{
		this.localChunkCoords = new Vector2I (x, y);
		this.worldCoords = new Vector2I (x + chunk.chunkCoords.x * Chunk.size, y + chunk.chunkCoords.y * Chunk.size);
		this.chunk = chunk;
		this.biome = biome;
	}

	public class BlockData {
		
	}
}