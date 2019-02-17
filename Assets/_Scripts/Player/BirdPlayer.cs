using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlayer : Player {

	private bool isFlying = false;

	protected override void Start ()
	{
		base.Start ();

		ability = new PlayerAbility (Fly, Walk, 2);
	}

	protected override void Update ()
	{
		if (isDead || !Game.isGameStarted || Game.isPause || isWaitingOnStart)
			return;
		
		if (blocksAround [0] == null) {
			CheckBlocksAndCollidersAround ((Direction)0);
		}

		for (int i = 0; i < 4; i++)
			if (MobileInputTouchManager.GetKey ((Direction)i, true)) {
				
				if (isFlying) {
					MoveFlying ((Direction)i);
				} else
					Move ((Direction)i);
			}

		if (ability != null)
			ability.Update ();
		
		UIScreen.Ins.UpdateScore (curBlock.worldCoords.x);

		if (isFlying)
			MoveFlying (Direction.Right);
	}

	protected override void FixedUpdate ()
	{
		if (isDead || !Game.isGameStarted || Game.isPause || isWaitingOnStart)
			return;

		if (isFlying) {
			if (moveProgress < 1) {
				transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (targetRotation), Time.fixedDeltaTime * 20);
				Vector3 pos = Vector3.Lerp (transform.position, targetPos, moveProgress);
				moveProgress += Time.fixedDeltaTime / (1f / speed);
				transform.position = pos;
			}
		} else
			MoveAnimate ();
	}

	void Fly ()
	{
		isFlying = true;
		Debug.Log ("Flying!");
		if (isSnaped)
			Unsnap (Direction.Right);
		targetRotation = Quaternion.LookRotation (new Vector3 (CubeMeshData.offsets [(int)Direction.Right].x, 0, CubeMeshData.offsets [(int)Direction.Right].y)).eulerAngles;
	}

	void Walk ()
	{
		isFlying = false;
		Move (Direction.Top);
		Debug.Log ("Walking!");
	}

	float lastFlyingTime = -1;
	float flyingHeight = 2.5f;

	void MoveFlying (Direction dir)
	{
		if (Time.time > lastFlyingTime + (1f / speed) + .01f) {
			CheckBlocksAndCollidersAround (dir);

			lastFlyingTime = Time.time;

			curCoord += CubeMeshData.offsets [(int)dir];

			targetPos = new Vector3 (curCoord.x, flyingHeight, curCoord.y);
			StartFlyingMove (dir, targetPos);
			moveProgress = 0;
		}
	}

	void StartFlyingMove (Direction dir, Vector3 targetPos)
	{
		moveProgress = 0;
		targetRotation = Quaternion.LookRotation (new Vector3 (CubeMeshData.offsets [(int)dir].x, 0, CubeMeshData.offsets [(int)dir].y)).eulerAngles;
		transform.localScale = new Vector3 (1.1f, .8f, 1.1f);
		targetScale = Vector3.one;
		this.targetPos = targetPos; //= new Vector3 (curCoord.x, startHeight + curBlock.GetBlockHeight (), curCoord.y);
		//if (curDir != Direction.Right && dirApply)
		dirApply = false;
	}
}
