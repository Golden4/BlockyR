using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalkCrocodile : Balk {
	
	protected override void Start ()
	{
		canSnap = true;
		float localSize = 1;//0.15f + curBalkLine.size * 0.3f;
		transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, localSize);
		size = 1;
		transform.localEulerAngles = new Vector3 (0, 180 * ((curBalkLine.dir == BalkDirection.Left) ? 0 : 1), 0);
	}

	public override void OnPlayerSnap ()
	{
		base.OnPlayerSnap ();

		Utility.Invoke (this, 0.8f, MoveDown);
		targetPos.y = -1.5f;
	}

	public override void OnPlayerUnsnap ()
	{
		base.OnPlayerUnsnap ();
		targetPos.y = .1f;

		GetComponent <BoxCollider> ().enabled = true;
	}

	private Vector3 targetPos;

	protected override void FixedUpdate ()
	{
		if (Game.isGameStarted || Player.Ins.isDead) {
			
			Vector3 pos = transform.position;
			pos.z += Time.fixedDeltaTime * curBalkLine.speed * ((int)curBalkLine.dir * 2 - 1);

			pos.y = Vector3.Lerp (pos, targetPos, Time.fixedDeltaTime / 2f).y;

			transform.position = pos;
		}
	}

	void MoveDown ()
	{
		if (isSnaped) {
			GetComponent <BoxCollider> ().enabled = false;
			Player.Ins.Move (Direction.Top);
		}
	}

	protected override void OnReuse ()
	{
		base.OnReuse ();
	}
}
