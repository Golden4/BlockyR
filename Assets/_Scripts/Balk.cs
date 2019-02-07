using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balk : MonoBehaviour {

	public enum BalkDirection {
		Left,
		Right
	}

	public MovingObjectsManager.BalksLine curBalkLine;

	public bool isOnChunk;

	public LayerMask lm;

	void Start ()
	{

		float localSize = 0.15f + curBalkLine.size * 0.3f;
		transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, localSize);


		//isOnChunk = World.IsOnChunk (transform.position);
	}

	void Update ()
	{
		isOnChunk = World.IsOnChunk (transform.position);
		if (!isOnChunk)
			OutOfChunk ();
	}

	void FixedUpdate ()
	{
		if (Game.isGameStarted || Player.Ins.isDead)
			Move ();

	}

	void Move ()
	{
		transform.position += Time.fixedDeltaTime * Vector3.forward * curBalkLine.speed * ((int)curBalkLine.dir * 2 - 1);
	}

	Vector2I curChunkCoord {
		get {
			return World.WorldPositionToChunkCoords (transform.position);
		}
	}

	void OutOfChunk ()
	{
		//bool chunkOut = (curBalkLine.dir == BalkDirection.Left) ? World.Ins.outChunkIndexMinY - 1 < curChunkCoord.y : World.Ins.outChunkIndexMaxY + 1 > curChunkCoord.y;

		//bool chunkOut = World.Ins.outChunkIndexMinY - 2 < curChunkCoord.x;

/*		if (!chunkOut) {
			if (MovingObjectsManager.Ins.balksLine.ContainsKey (curBalkLine.line))
				MovingObjectsManager.Ins.DestroyBalksInLine (curBalkLine.line);

			print (transform.name + "   " + curBalkLine.dir + "  " + curBalkLine.line + "  " + curChunkCoord + "    " + World.Ins.outChunkIndexMinY + "   " + World.Ins.outChunkIndexMaxY);
			return;
		}*/

		//if (chunkOut)
		if (MovingObjectsManager.Ins.balksLine.ContainsKey (curBalkLine.line)) {

			transform.position = MovingObjectsManager.GetStartBalkPosition (curBalkLine.line);

		}

	}
}
