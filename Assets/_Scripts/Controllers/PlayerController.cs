using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	void Awake ()
	{
		Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
		SpawnPlayer (User.GetInfo.curPlayerIndex, Vector3.up * .5f);

	}

	void SpawnPlayer (int index, Vector3 pos)
	{
		Player player = Database.Get.playersData [index].playerPrefab;

		Player spawnedPlayer = Instantiate (player.gameObject).GetComponent <Player> ();

		spawnedPlayer.transform.position = pos;

		spawnedPlayer.transform.localEulerAngles = new Vector3 (0, 180, 0);
	}
}
