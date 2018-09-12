using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWater : Block {

	public override bool CanDie ()
	{
		return (biome != Biome.Snowy);
	}

	public override Vector2I textureCoords (int dir)
	{
		return new Vector2I (2, (int)biome);
	}

	public override void MakeBlock (Vector3 pos, MeshData mesh)
	{
		float height = /*(biomeIndex == 2) ? 0.95f :*/ 0.3f;

		MakeFace (5, pos - Vector3.up * height, mesh);
	}

	public override DieInfo dieInfo ()
	{
		return DieInfo.Water;
	}

	public BlockWater (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
	}

	public override float GetBlockHeight ()
	{
		return (isWalkable () && !CanDie ()) ? -.1f : -1.5f;
	}
}
