using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItem : MonoBehaviour {

	public float rotationSpeed = 80;

	void Update ()
	{
		transform.localEulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
		Vector3 pos = transform.localPosition;
		pos.y = Mathf.Sin (Time.time * 1.5f) * 0.2f + 1;
		transform.localPosition = pos;
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			PickUp (col.GetComponent <Player> ());
			Destroy ();
		}
	}

	public virtual void PickUp (Player player)
	{
		
	}

	void Destroy ()
	{
		Destroy (gameObject);
	}
}
