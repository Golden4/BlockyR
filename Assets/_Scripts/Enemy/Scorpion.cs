using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpion : Enemy {

	public float speed = 5;
	public float stayingTime = 1;

	enum AIState
	{
		Moving,
		Staying
	}

	AIState curState = AIState.Moving;

	Block targetBlock;

	Block curBlock;

	float lastTime = -1;
	Vector2I curCoords;

	protected override void Update ()
	{
		if (!Game.isGameStarted)
			return;



		curCoords = new Vector2I (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.z));

		if (curBlock == null) {
			curBlock = World.Ins.GetBlock (curCoords);

			if (curBlock == null)
				return;

			biome = curBlock.biome;
		}

		if (curState == AIState.Moving) {

			if (targetBlock == null) {
				FindTargetBlock ((Direction)((Random.Range (0, 2) == 0) ? 0 : 2));
			} else {
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (targetBlock.worldCoords.x, transform.position.y, targetBlock.worldCoords.y), Time.deltaTime * speed);

				if (curCoords == targetBlock.worldCoords) {
					curState = AIState.Staying;
					lastTime = Time.time;
					targetBlock = null;
				}
			}
		}

		if (curState == AIState.Staying && Time.time > lastTime + stayingTime) {
			lastTime = Time.time;
			curState = AIState.Moving;
		}

		if (curState == AIState.Moving && targetBlock != null && (targetBlock.worldCoords - curCoords).ToVector3 (0) != Vector3.zero) {
			transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.LookRotation ((targetBlock.worldCoords - curCoords).ToVector3 (0)), Time.deltaTime * 5);
		}
		base.Update ();


	}

	void FindTargetBlock (Direction dir)
	{
		int randDist = Random.Range (10, 20);


		int temp = 1;

		for (int i = 1; i < randDist; i++) {
			Block block = World.Ins.GetBlock (curCoords + CubeMeshData.offsets [(int)dir] * i);

			if (!(block is BlockGrass) || block.biome != biome || !block.isWalkable ()) {
				temp = i - 1;
				break;
			}

			if (i == randDist - 1) {
				temp = i;
			}
		}

		targetBlock = World.Ins.GetBlock (curCoords + CubeMeshData.offsets [(int)dir] * temp);
	}

	void SetTargetBlock (Block block)
	{
		targetBlock = block;
	}

}
