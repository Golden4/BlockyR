using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class BlockBalk : BlockWater {

	public override Vector2I textureCoords (int dir)
	{
		int coord = (int)biome;
		if (biome == Biome.Snowy) {
			coord = 0;
		}

		return new Vector2I (2, coord);
	}

	public override bool CanDie ()
	{
		return true;
	}

	public BlockBalk (int x, int y, Chunk chunk, Biome biome) : base (x, y, chunk, biome)
	{
	}
	
}
