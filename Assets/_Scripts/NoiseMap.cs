using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour {
	public static NoiseMap Ins;

	public Vector2 offset;

	public float scale = .5f;
	[Range (0, 1f)]
	public float noiseHeight = .6f;

	public float seed = 100;

	public Material material;

	Texture2D texture;

	void Awake ()
	{
		Ins = this;
		offset.x = Random.Range (-999999, 999999);
		offset.y = Random.Range (-999999, 999999);
	}

	public static int[,] BiomesMap (int width, int height, int xOffset, int yOffset, int biomeCount = 3)
	{
		int[,] map = new int[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				
				bool colorAssigned = false;

				int biomeType = -1;

				for (int k = 0; k < biomeCount; k++) {
					float noiseValue = Mathf.PerlinNoise ((float)(i + xOffset) / width * Ins.scale + k * 10000 + Ins.offset.x, (float)(j + yOffset) / height * Ins.scale + k * 10000 + Ins.offset.y);

					if (noiseValue > Ins.noiseHeight && !colorAssigned) {
						biomeType = k;

						colorAssigned = true;

					}

					if (biomeType == -1 && !colorAssigned && k == biomeCount - 1) {
						biomeType = biomeCount - 1;
					}

					if (k == biomeCount - 1)
						map [i, j] = biomeType;
				}

			}
		}

		return map;
	}

	public static int[,] BlocksMap (int width, int height, int xOffset, int yOffset, ref int[,] biomesMap)
	{
		int[,] map = new int[width, height];

		int[,] levelMap = ObjectsMap (width, height, xOffset, yOffset);

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				if (x + xOffset >= -3 && x + xOffset <= 3 && y + yOffset >= -3 && y + yOffset <= 3) {
					map [x, y] = 0;
					continue;
				}

				if (Random.Range (0, 15) > 0)
					map [x, y] = 0;
				else if (Random.Range (0, 5) > 0) {
						map [x, y] = 2;
					} else if (biomesMap [x, y] != 1) {
							CreatePool (x, y, map);
							//map [x, y] = 1;
						} else {
							map [x, y] = 0;
						}


				/*	if (levelMap [x, y] == 1) {
					print (x + xOffset + "  " + (y + yOffset));
					map [x, y] = 1;
					biomesMap [y, x] = 0;

					BalkController.AddLineToGenerateBalks (yOffset + y);
				}*/

				if ((xOffset + y) % 20 > 0 && ((xOffset + y) % 20 + (xOffset + y) / 10) < (((xOffset + y) / 10) * 2) && (xOffset + y) > 10) {
					//if (xOffset + y > 20 + Random.Range (0, 2) && xOffset + y < 30 + Random.Range (0, 2)) {
					map [x, y] = 4;

					MovingObjectsManager.AddLineToGenerateBalks (xOffset + y);


				}
			}
		}

		return map;
	}

	static void CreatePool (int x, int y, int[,] map)
	{
		for (int i = -2; i <= 2; i++) {
			for (int j = -2; j <= 2; j++) {
				if (x + i >= 0 && x + i < map.GetLength (0) && y + j >= 0 && y + j < map.GetLength (1)) {
					if (Random.Range (0, 3) > 0)
						map [x + i, y + j] = 1;
				}
			}
		}
	}

	public static int[,] ObjectsMap (int width, int height, int xOffset, int yOffset)
	{
		int[,] map = new int[width, height];

		int level = 0;
		int offsetBetweenLevel = 10;

		List<int> listInts = new List<int> ();

		for (int j = 0; j < 8; j++) {
			level++;

			for (int i = 0; i < level; i++) {
				int num = offsetBetweenLevel * level + sum (level) * 2 + i * 2;
				listInts.Add (num);
			}
		}

		for (int y = 0; y < height; y++) {
			if (listInts.Contains (y + yOffset))
				CreateLine (y, map);
		}

		return map;
	}

	static int sum (int level)
	{
		int num = 0;

		num = level * (level - 1) / 2;

		return num;
	}

	static void CreateLine (int y, int[,] map)
	{
		for (int x = 0; x < map.GetLength (0); x++) {
			
			if (x % 2 == (y / 2) % 2) {
				map [x, y] = 1;
			} else {
				map [x, y] = 0;
			}
			
		}
	}

	#if UNITY_EDITOR

	void ShowInPlane (int[,] map)
	{
		
		int width = map.GetLength (0);
		int height = map.GetLength (1);
		texture = new Texture2D (width, height);

		Color[] colors = {
			Color.white, Color.blue, Color.green, Color.yellow, Color.red
		};

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				texture.SetPixel (i, j, colors [map [i, j]]);
			}
		}

		texture.Apply ();

		material.mainTexture = texture;
	}

	void OnGUI ()
	{
/*		if (GUI.Button (new Rect (new Vector2 (50, 50), new Vector2 (100, 50)), "Generate")) {
			offset.x = Random.Range (-999999, 999999);
			offset.y = Random.Range (-999999, 999999);
			ShowInPlane (BiomesMap (100, 100, 1, 1));
		
		}*/
	}
	#endif

}
