using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
	public static Fire Ins;

	public Sound fireSound;
	AudioSource source;

	public Player target;

	public float distanceToPlayer {
		get {
			return target.transform.position.x - distance - 2.8f;
		}
	}

	float distance = -4.5f;
	float maxDistance = 7f;

	float startPos = 4.5f;

	/*	public float speed {
		get {
			return 1f / Mathf.Clamp (UIScreen.Ins.score / 100f * 4 + 4, 4, 12);
		}
	}*/
	float speed = .8f;

	ParticleSystem[] ps;

	void Awake ()
	{
		Ins = this;
	}

	void Start ()
	{
		target = Player.Ins;
		Game.OnGameStarted += Game_OnGameStarted;
		Player.OnPlayerRetry += Player_OnPlayerRetry;
		source = GetComponent <AudioSource> ();
		ps = GetComponentsInChildren <ParticleSystem> ();
	}

	void Player_OnPlayerRetry ()
	{
		distance = target.transform.position.x - maxDistance;
		foreach (var particle in ps) {
			particle.Clear ();
		}
	}

	void Game_OnGameStarted ()
	{
		if (AudioManager.audioEnabled) {
			source.clip = fireSound.clip;
			source.Play ();
		}
	}

	void OnDestroy ()
	{
		Game.OnGameStarted -= Game_OnGameStarted;
		Player.OnPlayerRetry -= Player_OnPlayerRetry;
	}

	void Update ()
	{
		if (target.isDead || !Game.isGameStarted)
			return;
		
		if (distance < target.transform.position.x - maxDistance) {
			distance = target.transform.position.x - maxDistance;

		} else {
			distance += Time.deltaTime * speed;
		}

		transform.position = new Vector3 (distance, transform.position.y, target.transform.position.z);

		speed = Mathf.Clamp ((UIScreen.Ins.score / 250f) + 0.5f, 0.5f, 0.9f);

	}


	void OnTriggerEnter (Collider col)
	{
		if (col.CompareTag ("Player")) {
			col.GetComponentInParent <Player> ().Die (DieInfo.Fire);
		}
	}

}
