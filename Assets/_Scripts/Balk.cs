using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balk : MonoBehaviour {

	public enum BalkDirection
	{
		Left,
		Right
	}

	public BalksController.BalksLine curBalkLine;

	public bool isOnChunk;

	public bool canSnap = true;

	public int size;

	protected bool isSnaped = false;

	protected float moveDownDegree;
	protected float moveDownTime = 0.2f;

	protected float targetPosY = -.15f;
	protected float startPosY = .1f;

	protected virtual void Start ()
	{
		float localSize = 0.15f + size * 0.3f;
		transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, localSize);
		//isOnChunk = World.IsOnChunk (transform.position);
	}

	public virtual void OnPlayerSnap ()
	{
		isSnaped = true;
	}

	public virtual void OnPlayerUnsnap ()
	{
		isSnaped = false;
	}

	protected virtual void Update ()
	{
		isOnChunk = World.IsOnChunk (transform.position);
		if (!isOnChunk)
			OutOfChunk ();
	}

	protected virtual void FixedUpdate ()
	{
		if (Game.isGameStarted || Player.Ins.isDead)
			Move ();
	}

	public virtual void PlayerMovedOnBalk (Direction dir)
	{
		moveDownDegree = 0;
	}

	protected virtual void Move ()
	{
		Vector3 pos = transform.position;
		pos.z += Time.fixedDeltaTime * curBalkLine.speed * ((int)curBalkLine.dir * 2 - 1);
		moveDownDegree = Mathf.Clamp (moveDownDegree + ((isSnaped) ? 1 : -1) * Time.fixedDeltaTime / moveDownTime, 0, 2);
		pos.y = Mathf.Lerp (startPosY, targetPosY, 1 - Mathf.Abs (1 - moveDownDegree));
		transform.position = pos;
	}

	Vector2I curChunkCoord {
		get {
			return World.WorldPositionToChunkCoords (transform.position);
		}
	}

	void OutOfChunk ()
	{
		//bool chunkOut = (curBalkLine.dir == BalkDirection.Left) ? World.Ins.outChunkIndexMinY - 1 < curChunkCoord.y : World.Ins.outChunkIndexMaxY + 1 > curChunkCoord.y;

		/*bool chunkOut = World.Ins.outChunkIndexMinY - 2 < curChunkCoord.x;

		if (!chunkOut) {
			if (BalksController.Ins.balksLine.ContainsKey (curBalkLine.line))
				BalksController.Ins.DestroyBalksInLine (curBalkLine.line);

			print (transform.name + "   " + curBalkLine.dir + "  " + curBalkLine.line + "  " + curChunkCoord + "    " + World.Ins.outChunkIndexMinY + "   " + World.Ins.outChunkIndexMaxY);
			return;
		}*/

		//if (chunkOut)
		if (BalksController.Ins.balksLine.ContainsKey (curBalkLine.line)) {

			OnReuse ();

		}

	}

	protected virtual void OnReuse ()
	{
		transform.position = BalksController.GetStartBalkPosition (curBalkLine.line);
	}
}
