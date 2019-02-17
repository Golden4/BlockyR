using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {

	public float rotationSpeed = 80;

	public Vector3 startPosToMove;

	public Vector3 endPosToMove;
	public float speed = 10;

	float moveValue;
	float randomValue;

	public void Init (Vector3 startPosToMove, Vector3 endPosToMove)
	{
		this.startPosToMove = startPosToMove;
		this.endPosToMove = endPosToMove;
		randomValue = Random.Range (0, 100f);
	}


	void Update ()
	{
		Vector3 rot = transform.localEulerAngles;
		rot.y = 90;
		rot.z += rotationSpeed * Time.deltaTime;
		transform.localEulerAngles = rot;

		moveValue = Mathf.PingPong ((Time.time + randomValue) * speed, 1);
		moveValue += Time.deltaTime * speed;

		transform.position = Vector3.Lerp (startPosToMove, endPosToMove, moveValue);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			col.GetComponent <Player> ().Die (DieInfo.Trap);
		}
	}

}
