using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	protected Biome biome;
	public bool dead;

	protected virtual void Update ()
	{
		if (!World.IsOnChunk (transform.position) && !dead) {
			Die ();
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			col.GetComponentInParent <Player> ().Die (DieInfo.Enemy);
			Die ();
		}
	}

	void Die ()
	{
		dead = true;
		EnemyController.Ins.RemoveEnemy (this);
		Destroy (gameObject);
	}
}
