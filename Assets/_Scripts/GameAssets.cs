using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ()]
public class GameAssets : ScriptableObject {
	static GameAssets _i;

	public static GameAssets i {
		get {
			if (_i == null) {
				_i = Resources.Load <GameAssets> ("Assets");
			}

			return _i;
		}
	}

	public GameAsset asCoin;
	public GameAsset asAbility;
	public GameAsset asShipi;
	public GameAsset asBearTrap;

	public GameAsset asWaterLily;

	[System.Serializable]
	public class GameAsset {
		public string name;
		public GameObject pf;

		public GameAsset CreateOnWorld (Transform parent, Vector3 pos, Vector3 rot, bool isStatic = true)
		{
			GameObject obj = MonoBehaviour.Instantiate (pf);
			obj.transform.SetParent (parent, false);
			obj.transform.rotation = Quaternion.Euler (rot);
			obj.transform.position = pos;
			obj.isStatic = isStatic;
			return new GameAsset (name, obj);
		}

		public GameAsset (string name, GameObject go)
		{
			this.name = name;
			this.pf = go;
		}
		
	}

}
