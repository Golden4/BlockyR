using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalkCrocodile : Balk {

	protected override void Start ()
	{
		canSnap = true;
		moveDownTime = 1f;
		size = 2;
		targetPosY = -0.6f;
		float localSize = 1;
		transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, localSize);
		transform.GetChild (0).localEulerAngles = new Vector3 (0, 180 * ((curBalkLine.dir == BalkDirection.Left) ? 0 : 1), 0);
	}

	public override void OnPlayerSnap ()
	{
		base.OnPlayerSnap ();
	}

	public override void OnPlayerUnsnap ()
	{
		base.OnPlayerUnsnap ();

		GetComponent <BoxCollider> ().enabled = true;
	}

	public override void PlayerMovedOnBalk (Direction dir)
	{
	}

	protected override void Move ()
	{
		Vector3 pos = transform.position;
		pos.z += Time.fixedDeltaTime * curBalkLine.speed * ((int)curBalkLine.dir * 2 - 1);
		moveDownDegree = Mathf.Clamp01 (moveDownDegree + ((isSnaped) ? 1 : -1) * Time.fixedDeltaTime / moveDownTime);
		pos.y = Mathf.Lerp (startPosY, targetPosY, moveDownDegree);
		transform.position = pos;
	}

	protected override void Update ()
	{
		base.Update ();
		if (isSnaped && moveDownDegree >= 1) {
			GetComponent <BoxCollider> ().enabled = false;

			if (!Player.Ins.isDead)
				Player.Ins.Move (Direction.Top);
		}
	}

	protected override void OnReuse ()
	{
		base.OnReuse ();
	}
}
