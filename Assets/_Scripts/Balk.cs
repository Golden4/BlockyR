using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balk : MonoBehaviour {

	public enum BalkDirection
	{
		Left,
		Right
	}

	public float speed = .5f;

	public BalkDirection dir;

	public bool isOnChunk;

	public LayerMask lm;
	public int line;

	void Start ()
	{
		isOnChunk = World.IsOnChunk (transform.position);
	}

	void Update ()
	{
		if (!World.IsOnChunk (transform.position))
		if (isOnChunk)
			OutOfChunk ();

		isOnChunk = World.IsOnChunk (transform.position);
	}

	void FixedUpdate ()
	{
		if (Game.isGameStarted || Player.Ins.isDead)
			Move ();

	}

	void Move ()
	{
		transform.position += Time.fixedDeltaTime * Vector3.forward * speed * ((int)dir * 2 - 1);
	}

	Vector2I curChunkCoord {
		get {
			return World.WorldPositionToChunkCoords (transform.position);
		}
	}

	void OutOfChunk ()
	{
		//bool chunkOut = (dir == BalkDirection.Left) ? World.Ins.outChunkIndexMinY - 1 < curChunkCoord.y : World.Ins.outChunkIndexMaxY + 1 > curChunkCoord.y;

		bool chunkOut = World.Ins.outChunkIndexMinY - 2 < curChunkCoord.x;

		if (!chunkOut) {
			if (MovingObjectsManager.Ins.balks.ContainsKey (line))
				MovingObjectsManager.Ins.DestroyBalksInLine (line);

			print (transform.name + "   " + dir + "  " + line + "  " + curChunkCoord + "    " + World.Ins.outChunkIndexMinY + "   " + World.Ins.outChunkIndexMaxY);
			return;
		}

		if (MovingObjectsManager.Ins.balks.ContainsKey (line)) {
			
			transform.position = MovingObjectsManager.GetStartBalkPosition (line);

		}
	}

}
