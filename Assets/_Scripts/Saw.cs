using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {

	public float rotationSpeed = 80;

	public Vector3 startPosToMove;

	public Vector3 endPosToMove;
	public float speed = 10;

	Vector2I chunkCoords;

	float moveValue;
	float randomValue;

	public void Init (Vector3 startPosToMove, Vector3 endPosToMove, Vector2I chunkCoords)
	{
		this.startPosToMove = startPosToMove;
		this.endPosToMove = endPosToMove;
		randomValue = Random.Range (0, 100f);
		this.chunkCoords = chunkCoords;
		Chunk.OnDestroyChunk += Chunk_OnDestroyChunk;

	}

	void Chunk_OnDestroyChunk (Vector2I obj)
	{
		if (chunkCoords == obj) {

			Destroy (gameObject);
		}
	}

	void OnDestroy ()
	{
		Chunk.OnDestroyChunk -= Chunk_OnDestroyChunk;
		Player.OnPlayerRetry -= OnPlayerRetry;
	}


	void FixedUpdate ()
	{
		Vector3 rot = transform.localEulerAngles;
		rot.y = 90;
		rot.z += rotationSpeed * Time.fixedDeltaTime;
		transform.localEulerAngles = rot;

		moveValue = Mathf.PingPong ((Time.time + randomValue) * speed, 1);
		moveValue += Time.fixedDeltaTime * speed;

		transform.position = Vector3.Lerp (startPosToMove, endPosToMove, moveValue);
	}

	void OnPlayerRetry ()
	{
		Destroy (gameObject);
	}



	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			Player.OnPlayerRetry += OnPlayerRetry;
			col.GetComponent <Player> ().Die (DieInfo.Trap);

		}
	}

}
