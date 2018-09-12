using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeMeshData {

	public static Vector3[] vertices = {
		new Vector3 (1, 1, 1),
		new Vector3 (-1, 1, 1),
		new Vector3 (-1, -1, 1),
		new Vector3 (1, -1, 1),
		new Vector3 (-1, 1, -1),
		new Vector3 (1, 1, -1),
		new Vector3 (1, -1, -1),
		new Vector3 (-1, -1, -1)
	};

	public static int[][] faceTriangles = {
		new int[]{ 0, 1, 2, 3 },
		new int[]{ 5, 0, 3, 6 },
		new int[]{ 4, 5, 6, 7 },
		new int[]{ 1, 4, 7, 2 },
		new int[]{ 5, 4, 1, 0 },
		new int[]{ 5, 4, 1, 0 }
	};

	public static Vector3[] faceVertices (int dir, float scale, Vector3 pos)
	{
		Vector3[] fv = new Vector3[4];
		for (int i = 0; i < fv.Length; i++) {
			fv [i] = vertices [faceTriangles [dir] [i]] * scale + pos;
		}
		return fv;
	}

	public static Vector2I[] offsets = {
		new Vector2I (0, 1),
		new Vector2I (1, 0),
		new Vector2I (0, -1),
		new Vector2I (-1, 0)
	};

	public static Vector2[] _uvs = {
		new Vector2 (0, 1),
		new Vector2 (1, 1),
		new Vector2 (1, 0),
		new Vector2 (0, 0)
	};

	public static Vector2[] faceUvs (int tileX, int tileY, float tilePerc)
	{
		Vector2[] uvsTemp = new Vector2[4];
		for (int i = 0; i < uvsTemp.Length; i++) {
			uvsTemp [i] = new Vector2 (tileX + _uvs [i].x, tileY + _uvs [i].y) * tilePerc;
		}
		return uvsTemp;
	}
}

public enum Direction
{
	Up,
	Right,
	Down,
	Left,
	Top,
	Bottom
}
