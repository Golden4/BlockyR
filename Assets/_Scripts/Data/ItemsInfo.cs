﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ()]
public class ItemsInfo : ScriptableObject {
	public ItemData[] playersData;

	[System.Serializable]
	public class ItemData {
		public string name;
		public string purchaseID;
		public int price;
		public Player playerPrefab;
		public string ability;
	}

}
