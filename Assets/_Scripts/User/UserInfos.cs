using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ()]
public class UserInfos : ScriptableObject {

	public UserData[] userData;

	public int curPlayerIndex = 0;

	[System.Serializable]
	public class UserData {
		public bool bought;
	}
}
