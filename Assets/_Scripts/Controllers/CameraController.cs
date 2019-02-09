using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Player target;
	public Vector3 offset;

	Camera curCamera;
	float fromFOV;

	void Start ()
	{
		target = Player.Ins;
		offset = transform.position - target.transform.position;
		curCamera = GetComponent <Camera> ();
		fromFOV = curCamera.fieldOfView;
		Player.OnPlayerDie += Player_OnPlayerDie;
		Player.OnPlayerRetry += Player_OnPlayerRetry;
	}

	void Player_OnPlayerRetry ()
	{
		StopAllCoroutines ();
		StartCoroutine (FocusToPlayer (fromFOV, 2f));
		//curCamera.fieldOfView = fromFOV;
	}

	void Player_OnPlayerDie ()
	{
		StopAllCoroutines ();
		StartCoroutine (FocusToPlayer (fromFOV - 15, .4f));
	}

	Vector3 velocity;

	void Update ()
	{
		Vector3 targetPos = Vector3.zero;
			
		float smoothTime = 0.01f;
		targetPos = new Vector3 (target.transform.position.x, 0, target.transform.position.z) + offset;

		transform.position = Vector3.SmoothDamp (transform.position, targetPos, ref velocity, smoothTime);
		//transform.position = Vector3.SmoothDamp (transform.position, new Vector3 (target.position.x, 0, target.position.z) + offset, ref velocity, Time.deltaTime * 20);

		/*if (distance <= target.position.x)
			distance = target.position.x;
		else
			distance += Time.deltaTime * .2f;*/
	}

	void OnDestroy ()
	{
		Player.OnPlayerDie -= Player_OnPlayerDie;
		Player.OnPlayerRetry -= Player_OnPlayerRetry;
	}

	bool focusing = false;

	IEnumerator FocusToPlayer (float targetFOV, float focusTime = .4f)
	{
		focusing = true;

		float progress = 0;

		while (progress < 1) {
			
			progress += Time.deltaTime / focusTime;	

			curCamera.fieldOfView = Mathf.Lerp (curCamera.fieldOfView, targetFOV, progress);
			yield return null;
		}

		curCamera.fieldOfView = targetFOV;

		focusing = false;

	}
}
