using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadHelper {

	public static void SaveToFile<T> (T info) where T : class
	{
		BinaryFormatter bf = new BinaryFormatter ();

		Directory.CreateDirectory (Application.persistentDataPath + "/Data/");

		FileStream file = File.Create (Application.persistentDataPath + "/Data/" + typeof(T).Name + ".dat");
		bf.Serialize (file, info);
		file.Close ();

		Debug.Log (Application.persistentDataPath + "/Data/" + typeof(T).Name + ".dat");
	}

	public static bool LoadFromFile<T> (out T data) where T : class
	{
		data = default(T);

		string path = Application.persistentDataPath + "/Data/" + typeof(T).Name + ".dat";

		File.Delete (path);

		if (File.Exists (path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);
			data = (T)bf.Deserialize (file);

			file.Close ();
			return true;
		}

		return false;
	}

}
